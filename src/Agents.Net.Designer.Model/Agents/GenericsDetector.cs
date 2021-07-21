#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Model.Agents
{
    [Intercepts(typeof(ModificationRequestExtending))]
    [Consumes(typeof(ModelVersionCreated))]
    public class GenericsDetector : InterceptorAgent
    {
        private readonly MessageCollector<ModificationRequestExtending, ModelVersionCreated> collector = new();
        private readonly Regex genericParser = new (@"^(?<generic_type>[^<]*)<(?<types>[^>]*)>$", RegexOptions.Compiled);
        public GenericsDetector(IMessageBoard messageBoard)
            : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }

        protected override InterceptionAction InterceptCore(Message messageData)
        {
            InterceptionAction action = InterceptionAction.Delay(out InterceptionDelayToken token);
            collector.PushAndContinue(messageData, set =>
            {
                set.MarkAsConsumed(set.Message1);

                set.Message1.RegisterExtender(list => ExtendModifications(list, set.Message2.Model));
                action = InterceptionAction.Continue;
                token.Release();
            }); 
            return action;
        }

        private bool ExtendModifications(List<Modification> modifications, CommunityModel model)
        {
            bool modified = false;
            foreach (Modification modification in modifications.ToArray())
            {
                bool newMessage = modification.ModificationType == ModificationType.Add &&
                                  modification.Target is CommunityModel &&
                                  modification.Property is PackageMessagesProperty;
                // bool newMessageName = modification.ModificationType == ModelModification.Change &&
                //                       modification.Property is MessageNameProperty;
                if (!newMessage || 
                    modification.NewValue is not MessageModel oldModel ||
                    !IsGeneric(oldModel.Name, model, out string @namespace) ||
                    oldModel.IsGenericInstance)
                {
                    continue;
                }

                MessageModel newMessageModel = oldModel.Clone(@namespace: @namespace, isGenericInstance: true);
                Modification addMessage = new(modification.ModificationType,
                                              modification.OldValue,
                                              newMessageModel,
                                              modification.Target,
                                              modification.Property);
                modifications.Insert(modifications.IndexOf(modification), addMessage);
                modifications.Remove(modification);
                modified = true;
            }

            return modified;
        }

        private bool IsGeneric(string name, CommunityModel model, out string @namespace)
        {
            @namespace = string.Empty;
            Match genericMatch = genericParser.Match(name);
            if (!genericMatch.Success)
            {
                return false;
            }

            int parameterCount = genericMatch.Groups["types"].Value.Count(c => c == ',')+1;
            MessageModel sourceType =
                model.Messages.FirstOrDefault(m => m.Name == genericMatch.Groups["generic_type"].Value && m.IsGeneric && m.GenericParameterCount == parameterCount);
            if (sourceType == null)
            {
                return false;
            }

            @namespace = sourceType.Namespace;
            return true;
        }
    }
}