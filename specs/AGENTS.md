# Instructions for Agents into specs folder

This file contains instructions for agents that are used in the project. Please follow these guidelines to ensure consistency and clarity in your contributions.

The `specs` folder is intended to house all specification documents related to the project. This includes, but is not limited to, functional requirements, technical specifications, design documents, task planning and breakdown, and any other relevant documentation that outlines the project's objectives and implementation strategies.

## Structure

- `specs/memory` folder: Contains documents that provide guidelines and best practices for the project, and should not be modified by agents.
  - `Constitution.md`: Outlines the foundational principles and responsibilities for the project. It MUST be considered by agents every time they make a change to any file in the repository.
- `specs/templates` folder: Contains templates for various types of documents that agents may need to create or modify, like requirements, design documents, and task breakdowns, or even source code file templates.
  - The templates should be referenced in prompt and instructions files for agents for consistency.
  - The templates should not be modified by agents.
  - The templates, in their content, use the format `[[Placeholder]]` to indicate where specific information should be filled in by agents.
  - The content of the templates is a strong suggestion and samples of how to write the documents, but agents can modify them as needed to fit the specific context of the feature or task they are working on.
- `specs/features` folder: Contains sub-folders for each feature of the project.
  - The name of the sub-folder should match the name of the feature and the git branch where the feature is being developed.
  - The format of the feature name should be `feature/feature-name`.
  - Each feature sub-folder can contain multiple files and folders to document the feature.
