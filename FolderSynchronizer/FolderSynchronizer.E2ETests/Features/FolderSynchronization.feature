Feature: Folder synchronization

  Scenario: Copy files from source folder to replica folder
    Given a source folder
    And the source folder contains the following files:
      | file            |
      | E2E-TC-1.txt    |
      | E2E-TC-1.csv    |
      | E2E-TC-1.png    |
    And the source folder contains the following folders:
      | folder              |
      | empty-folder        |
      | nested-1/nested-2   |
    And the folder "nested-1/nested-2" in the source contains the following files:
      | file         |
      | E2E-TC-1.txt |
      | E2E-TC-1.csv |
    And an empty replica folder
    When I run the folder synchronizer
    Then the replica folder contains the following folders:
      | folder            |
      | empty-folder      |
      | nested-1/nested-2 |
    And the replica folder contains the following files:
      | file            |
      | E2E-TC-1.txt    |
      | E2E-TC-1.csv    |
      | E2E-TC-1.png    |
    And the folder "nested-1/nested-2" in the replica contains the following files:
		| file         |
		| E2E-TC-1.txt |
		| E2E-TC-1.csv |
