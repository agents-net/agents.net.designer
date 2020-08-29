Feature: Automatic Serialization Use Case
	In order to save my work continuously
    As a software architect
    I want to automatically save after each modification

@enableLogging
Scenario: Save file after modification
In this scenario the model will be modified after a file was connected. 
Expected is that the modification is than saved to the connected file.
	Given I have started the designer
	And I connected the file "EmptyModel.amodel"
	When I add an agent to the model
	Then the file "EmptyModel.amodel" was updated
    And the model file "EmptyModel.amodel" looks like the file "SingleAgent.amodel"