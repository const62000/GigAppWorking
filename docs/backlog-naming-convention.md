## Naming Convention Guide for Features and User Stories

### General Guidelines

1. **Clarity**: Names should clearly describe the functionality or requirement.
2. **Consistency**: Use the same terminology throughout the document.
3. **Brevity**: Keep names concise while still conveying the necessary information.
4. **CamelCase**: Use CamelCase for multi-word names, starting with a capital letter.

### Features

- **Format**: `Feature [Feature Number]: [Feature Title]`
- **Example**:
  - `Feature 1: User Management`
  - `Feature 2: Facility Registration`

### User Stories

- **Format**: `User Story [User Story Number] ([M#]): As a [User Role], I want to [Action] so that [Benefit].`
- **Example**:
  - `User Story 1.1 (M1): As an Administrator, I want to create vendors and manage their basic information so that I can ensure accurate vendor records.`
  - `User Story 4.1 (M1): As a Job Manager, I want to log into the Caregiver mobile app so that I can post job details.`

### Acceptance Criteria

- **Format**: Use bullet points prefixed with a dash `-` for each acceptance criterion.
- **Example**:
  - `- The user can view, edit, and delete existing vendors.`
  - `- The system allows the administrator to update the basic information of an existing vendor.`

### Sub-types and Roles

- **Format**: Use descriptive names for roles and sub-types, following the CamelCase convention.
- **Example**:
  - `FreelancerCaregiver`
  - `VendorManager`

### Notes and Comments

- **Format**: Use a clear prefix for notes or comments, such as `Note:` or `Update note:`.
- **Example**:
  - `Note: This feature is critical for user onboarding.`
  - `Update note: This was simplified from an earlier version.`

### Versioning

- **Format**: Use a date format for updates, e.g., `Updated: Jan 31, 2025`.
- **Example**:
  - `- Updated: Jan 31, 2025`

### Example Structure

Here’s how a complete feature and user story might look following this guide:

```markdown
## Feature 1: User Management

- **Updated**: Jan 31, 2025

### User Story 1.1 (M1):

As an Administrator, I want to create vendors and manage their basic information so that I can ensure accurate vendor records.

- **Acceptance Criteria**:
  - The user can view, edit, and delete existing vendors.
  - The system allows the administrator to update the basic information of an existing vendor.
```

```

This update has been made to the `backlog-naming-convention.md` file. If you need any further modifications or additional content, feel free to ask!
```
