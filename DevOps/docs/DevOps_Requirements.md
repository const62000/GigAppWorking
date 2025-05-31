# DevOps

## Current State

- 1 `main` Branch
- Trunk Based Development
- 1 SQL Database: [gig_app_dev_db](https://portal.azure.com/#@purescale.ai/resource/subscriptions/87307f66-b2b9-4afd-a82e-524e24cd6491/resourceGroups/gig-app-dev-resourcegroupa/providers/Microsoft.Sql/servers/gigdev/databases/gig_app_dev_db/overview)
- CI/CD Pipeline: main --> API AppServices [gig-api-stage](https://portal.azure.com/#@purescale.ai/resource/subscriptions/87307f66-b2b9-4afd-a82e-524e24cd6491/resourceGroups/gig-app-dev-resourcegroupa/providers/Microsoft.Web/sites/gig-api-stage)


### Azure Dev Environment

- 1 Subscription: 
  - [Gig App Subscription Dev](https://portal.azure.com/#@purescale.ai/resource/subscriptions/87307f66-b2b9-4afd-a82e-524e24cd6491/resourceGroups/gig-app-dev-resourcegroupa/overview)
  - Subscription ID: 87307f66-b2b9-4afd-a82e-524e24cd6491
- 1 Azure Resource Group: [gig-app-dev-resourcegroup](https://portal.azure.com/#@purescale.ai/resource/subscriptions/87307f66-b2b9-4afd-a82e-524e24cd6491/resourceGroups/gig-app-dev-resourcegroupa/overview)

## Target **Interim State 1**

Same as current state except:
- **Repository**: Create a `release` Branch from `main`
- **Resource Group**: Create a `gig-app-stage-resourcegroup`
- **Database**: Create a `GigApp.Database` project in the solution.
  - [Create the project](https://learn.microsoft.com/en-us/sql/tools/sql-database-projects/get-started?view=sql-server-ver16&pivots=sq1-visual-studio-code#step-2-add-objects-to-the-project)
  - [Import the schema](https://learn.microsoft.com/en-us/sql/tools/sql-database-projects/howto/compare-database-project?view=sql-server-ver16&pivots=sq1-visual-studio-code)
- **Database**: Create a Staging SQL Database: `gig_app_stage_db` and add it to the `gig-app-stage-resourcegroup`.
- **CI/CD**: Create a CI/CD Pipeline for the `release` Branch that will deploy `GigApp.Database` to `gig_app_stage_db`.

## Target **Interim State 2**

Same as Interim State 1 except:
- **API AppService**: Create a `gig-api-stage` AppService and add it to the `gig-app-stage-resourcegroup`.
- **CI/CD**: Create a CI/CD Pipeline for the `release` Branch that will deploy `GigApp.API` to `gig-api-stage`.