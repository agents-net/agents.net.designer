Feature: Core Use Cases
	These are the core features of the designer.
    If they do not work the designer is considered broken.

Scenario: Save file after modification
In this scenario the model will be modified after a file was connected. 
Expected is that the modification is than saved to the connected file.
	Given I have started the designer
	And I connected the file "EmptyModel.amodel"
	When I add an agent to the model
	Then the file "EmptyModel.amodel" was updated
    And the model file "EmptyModel.amodel" looks like the file "SingleAgent.amodel"

Scenario: Show new message in view model
In this scenario the model will be modified. 
Expected is that the modification is shown in the view model.
	Given I have started the designer
	When I add a message to the model
	Then the tree contains the message "MessageX"