# Copilot Instructions

## Project Guidelines
- Architecture preference: DAL should depend on LogicLayer, not the other way around. Use dependency injection for services. This indicates a typical layered architecture where domain models are in LogicLayer and data access layer depends on them.