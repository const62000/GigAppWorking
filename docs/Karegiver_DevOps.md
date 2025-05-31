# Karegiver DevOps
- Presentation URL: https://gamma.app/docs/Karegiver-DevOps-hw9kpsia61dpkll

## Application components and their environments

| Application Component | Branch | Production Environment  | Staging Environment     | Development Environment | Notes                           |
| --------------------- | ------ | ----------------------- | ----------------------- | ----------------------- | ------------------------------- |
| Postgress DB          | main   | Add info when available | Add info when available | Add info when available |                                 |
| Auth0                 | main   | Add info when available | Add info when available | Add info when available |                                 |
| GigApp.Api            | main   | Add info when available | Add info when available | Add info when available |                                 |
| VMS.Client            | main   | Add info when available | Add info when available | Add info when available |                                 |
| GigApp.VMS            | main   | Add info when available | Add info when available | Add info when available |                                 |
| AWS KMS               | main   | Add info when available | Add info when available | Add info when available | KMS keys for secrets encryption |
| AWS Parameter Store   | main   | Add info when available | Add info when available | Add info when available | Environment-specific secrets    |



## Interrim State
Production environment setup but no DevOps pipeline.

### Production Environemnt Setup FIRST SETUP

**(ONLY ONCE, different procedure for future production updates)**

1. **Postgress**: Create Postgres database instance and initialize dataabase from [Create_Database_PostgreSQL.sql](database/PostgreSQL/Create_Database_PostgreSQL.sql) and Update [appsettings.json](/Users/saif/dev/GigApp.Backend/GigApp.Api/appsettings.json)
2. **Auth0**: Create Auth0 Instance and Update [appsettings.json](/Users/saif/dev/GigApp.Backend/GigApp.Api/appsettings.json)
3. **API**: Deploy API to AWS and test
4. **VMS**: Deploy web app to AWS and test
5. **iOS App**: Deploy iOS app to App Store Connect and test
6. **Android App**: Deploy Android app to Google Play and test



## DevOps Final State

### DevOps Fundamentals

1. **AWS KMS Setup**

   - Create AWS KMS key for each environment (dev/staging/prod)
   - Configure IAM roles and policies for KMS access
   - Set up key rotation policies

2. **Environment Configuration**

   - Create separate AWS Parameter Store entries for each environment
   - Store sensitive values (API keys, connection strings, etc.) in Parameter Store
   - Use KMS encryption for all sensitive parameters

3. **Application Integration**

   - Update application to use AWS Parameter Store for configuration
   - Implement environment-specific parameter paths
   - Configure AWS SDK credentials for Parameter Store access

4. **CI/CD Integration**
   - Update deployment pipelines to use environment-specific KMS keys
   - Configure build environments with appropriate AWS credentials
   - Implement secret rotation in deployment process

### Environment Creation Order
1. Production
2. Staging
3. Development
4. Feature/Branch

### Branching Strategy

#### Branch Structure

- `main` - Production branch, always contains production-ready code
- `staging` - Staging branch, mirrors production for testing
- `develop` - Development branch, contains latest development changes
- `feature/*` - Feature branches for new development
- `hotfix/*` - Hotfix branches for urgent production fixes

#### Workflow

1. **Development Flow**

   - Create feature branches from `develop`
   - Complete work and create PR to `develop`
   - After review, merge to `develop`

2. **Staging Deployment**

   - Create PR from `develop` to `staging`
   - After review, merge to `staging`
   - Automated deployment to staging environment

3. **Production Deployment**

   - Create PR from `staging` to `main`
   - After review, merge to `main`
   - Automated deployment to production environment

4. **Hotfix Process**
   - Create `hotfix/*` branch from `main`
   - Fix and test
   - Create PR to `main`
   - After review, merge to `main`
   - Create PR to `staging` and `develop`
   - Delete hotfix branch

### Branch Protection Rules

- `main`: Require PR reviews, status checks, and branch up-to-date
- `staging`: Require PR reviews and status checks
- `develop`: Require PR reviews and status checks