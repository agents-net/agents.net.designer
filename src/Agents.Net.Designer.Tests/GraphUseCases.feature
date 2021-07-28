Feature: GraphUseCases
These are the use cases regarding the shown graph.

    Scenario: Start graph in namespace scope
        Given I have started the designer
        And I connected the file "ComplexExampleWithMultipleNamespaces.amodel"
        Then the scope of the graph is "Namespace"

    Scenario: Show root namespace at the start
        Given I have started the designer
        And I connected the file "ComplexExampleWithMultipleNamespaces.amodel"
        Then the graph shows the following objects:
          | DisplayName |
          | SomeAgent   |
          | SomeMessage |
          | Finisher    |
        And the graph does not show the following objects:
          | DisplayName            |
          | AnotherInternalMessage |
          | SubRoutineFinisher     |
          | RoutineFinished        |
          | InternalMessage        |

    Scenario: Show connected objects from other namespaces
        Given I have started the designer
        And I connected the file "ComplexExampleWithMultipleNamespaces.amodel"
        Then the graph shows the following objects:
          | DisplayName                 |
          | OtherAgentActor             |
          | SpecialDecoratorInterceptor |
          | SpecialDecorator            |

    Scenario: Show only connected built-in types
        Given I have started the designer
        And I connected the file "ComplexExampleWithMultipleNamespaces.amodel"
        Then the graph shows the following objects:
          | DisplayName       |
          | InitializeMessage |
        And the graph does not show the following objects:
          | DisplayName       |
          | MessageAggregated |
          | ExceptionMessage  |

    Scenario: Switch selection changes namespace scope
        Given I have started the designer
        And I connected the file "ComplexExampleWithMultipleNamespaces.amodel"
        When I select the agent "OtherAgentActor"
        Then the graph shows the following objects:
          | DisplayName            |
          | AnotherInternalMessage |
          | SubRoutineFinisher     |
          | RoutineFinished        |
          | OtherAgentActor        |
          | SomeOtherMessage       |
          | OtherRoutineFinished   |
        And the graph does not show the following objects:
          | DisplayName                 |
          | InitializeMessage           |
          | Finisher                    |
          | SpecialDecoratorInterceptor |

    Scenario: Switch view scope to solution shows all namespaces
        Given I have started the designer
        And I connected the file "ComplexExampleWithMultipleNamespaces.amodel"
        When I switch the graph view scope to "Solution"
        Then the graph shows the following objects:
          | DisplayName            |
          | SomeAgent              |
          | SomeMessage            |
          | Finisher               |
          | AnotherInternalMessage |
          | SubRoutineFinisher     |
          | RoutineFinished        |
          | InternalMessage        |

    Scenario: Show only connected built-in types in solution view scope
        Given I have started the designer
        And I connected the file "ComplexExampleWithMultipleNamespaces.amodel"
        When I switch the graph view scope to "Solution"
        Then the graph shows the following objects:
          | DisplayName       |
          | InitializeMessage |
        And the graph does not show the following objects:
          | DisplayName       |
          | MessageAggregated |
          | ExceptionMessage  |

    Scenario: Consider selection when switching from solution scope to namespace scope
        Given I have started the designer
        And I connected the file "ComplexExampleWithMultipleNamespaces.amodel"
        When I select the agent "OtherAgentActor"
        And I switch the graph view scope to "Solution"
        And I switch the graph view scope to "Namespace"
        Then the graph shows the following objects:
          | DisplayName            |
          | AnotherInternalMessage |
          | SubRoutineFinisher     |
        And the graph does not show the following objects:
          | DisplayName                 |
          | InitializeMessage           |
          | Finisher                    |
          | SpecialDecoratorInterceptor |