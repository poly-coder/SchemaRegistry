# Instructions for Agents into specs folder

This file contains instructions for agents that are used in the project. Please follow these guidelines to ensure consistency and clarity in your contributions.

The `specs` folder is intended to house all specification documents related to the project. This includes, but is not limited to, functional requirements, technical specifications, design documents, task planning and breakdown, and any other relevant documentation that outlines the project's objectives and implementation strategies.

## Structure

- `specs/memory` folder: Contains documents that provide guidelines and best practices for the project, and should not be modified by agents.
  - `Constitution.md`: Outlines the foundational principles and responsibilities for the project. It MUST be considered by agents every time they make a change to any file in the repository.
- `specs/features` folder: Contains sub-folders for each feature of the project.
  - The name of the sub-folder should match the name of the feature and the git branch where the feature is being developed.
  - The format of the feature name should be `feature/feature-name`.
  - Each feature sub-folder should contain the following files:
    - `README.md`: Provides an overview of the feature, its purpose, and any relevant information.
    - `Requirements.md`: Lists the specific requirements and acceptance criteria for the feature.
    - `Design.md`: Details the design and architecture of the feature, including diagrams and explanations.
    - `Implementation.md`: Describes the implementation details, including code snippets and explanations.
    - `Tasks.md`: Breaks down the feature into smaller tasks, with descriptions and estimated time for completion.
    - `assets` folder: Contains any assets related to the feature, such as images, diagrams, or other resources.
