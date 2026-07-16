# Healtive Database Design

Version: 1.0

Author: Komal Kale

Architecture: Multi-Tenant SaaS

Database: MySQL

---

# Overview

Healtive is a cloud-based multi-tenant Hospital Management System (SaaS).

Each hospital works independently while sharing the same application.

The platform is managed by the Healtive Super Admin.

Every hospital manages its own:

- Branches
- Users
- Roles
- Doctors
- Patients
- Appointments
- Billing
- Pharmacy
- Laboratory

Patients have a global Healtive profile and can visit multiple hospitals.

The system is designed to support Web and Mobile applications using a shared .NET Web API.

# Module 1 – SaaS Management

Purpose

This module is responsible for managing hospitals that subscribe to Healtive.

A hospital is the primary tenant of the system.

Each hospital can have one or more branches.

The module also manages subscription plans, billing cycles, trial periods, and activation status.

## Business Rules

1. Every hospital is an independent tenant.

2. A hospital may have one or many branches.

3. Branches belong to only one hospital.

4. Hospital data must never be visible to another hospital.

5. Each hospital has its own users, roles, permissions, doctors, patients and appointments.

6. A hospital subscription controls access to the software.

7. If a subscription expires, hospital users cannot log in until the subscription is renewed.

8. Healtive Super Admin manages hospitals but does not participate in hospital operations.

9. Patients have a global Healtive account and may visit multiple hospitals.

10. Each hospital stores its own medical records for patient visits.

# Table of Contents

1. Overview
2. Architecture
3. Module 1 – SaaS Management
4. Module 2 – Authentication & Authorization
5. Module 3 – Organization
6. Module 4 – Patient Management
7. Module 5 – Appointment Management
8. Module 6 – Prescription Management
9. Module 7 – Billing
10. Module 8 – Pharmacy
11. Module 9 – Laboratory
12. Module 10 – Notifications
13. Module 11 – Audit Logs

## High Level Architecture

Healtive (Super Admin)
        │
        ▼
+------------------+
|    Hospital      |
+------------------+
        │
        ▼
+------------------+
|     Branch       |
+------------------+
        │
        ▼
+------------------+
|      Users       |
+------------------+
        │
        ├── Roles
        ├── Doctors
        ├── Receptionists
        ├── Pharmacists
        ├── Patients
        ├── Billing
        ├── Laboratory
        └── Pharmacy


        ## Table: Hospitals

### Purpose

Stores information about every hospital registered on the Healtive platform.

Each hospital represents one tenant.

---

### Columns

| Column | Type | Required | Description |
|---------|------|----------|-------------|
| Id | Guid | Yes | Primary Key |
| Name | varchar(200) | Yes | Hospital Name |
| Code | varchar(50) | Yes | Unique Hospital Code |
| LicenseNumber | varchar(100) | Yes | Government Registration Number |
| GSTNumber | varchar(50) | No | Tax Registration Number |
| HospitalType | varchar(100) | Yes | General, Dental, Eye, Multi-speciality etc. |
| Email | varchar(150) | Yes | Official Email |
| PhoneNumber | varchar(20) | Yes | Contact Number |
| Website | varchar(200) | No | Hospital Website |
| LogoUrl | varchar(300) | No | Logo Path |
| Address | varchar(300) | Yes | Address |
| City | varchar(100) | Yes | City |
| State | varchar(100) | Yes | State |
| Country | varchar(100) | Yes | Country |
| PostalCode | varchar(20) | No | Postal Code |
| TimeZone | varchar(100) | Yes | Default Time Zone |
| Currency | varchar(20) | Yes | INR, USD etc. |
| IsActive | bit | Yes | Active / Inactive |
| CreatedAt | datetime | Yes | Created Date |
| UpdatedAt | datetime | No | Updated Date |
| IsDeleted | bit | Yes | Soft Delete |

### Relationships

Hospital

↓

One Hospital

↓

Many Branches

↓

Many Users

↓

Many Departments

↓

Many Doctors

↓

Many Appointments

## Table: SubscriptionPlans

### Purpose

Stores all subscription plans offered by Healtive.

Examples:

- Free Trial
- Starter
- Professional
- Enterprise

---

### Columns

| Column | Type | Required | Description |
|---------|------|----------|-------------|
| Id | Guid | Yes | Primary Key |
| Name | varchar(100) | Yes | Plan Name |
| Description | varchar(500) | No | Plan Details |
| Price | decimal(10,2) | Yes | Plan Price |
| DurationInDays | int | Yes | Subscription Duration |
| MaxBranches | int | Yes | Allowed Branches |
| MaxDoctors | int | Yes | Allowed Doctors |
| MaxPatients | int | Yes | Allowed Patients |
| IsTrial | bit | Yes | Trial Plan |
| IsActive | bit | Yes | Active Status |
| CreatedAt | datetime | Yes | Created Date |

## Table: HospitalSubscriptions

### Purpose

Stores the active subscription for each hospital.

---

### Columns

| Column | Type | Required | Description |
|---------|------|----------|-------------|
| Id | Guid | Yes | Primary Key |
| HospitalId | Guid | Yes | FK → Hospitals.Id |
| SubscriptionPlanId | Guid | Yes | FK → SubscriptionPlans.Id |
| StartDate | datetime | Yes | Subscription Start |
| EndDate | datetime | Yes | Subscription Expiry |
| TrialEndsOn | datetime | No | Trial Expiry |
| AmountPaid | decimal(10,2) | Yes | Amount Paid |
| PaymentStatus | varchar(50) | Yes | Paid / Pending / Failed |
| IsActive | bit | Yes | Active Subscription |
| CreatedAt | datetime | Yes | Created Date |

SubscriptionPlans
        │
        │ 1
        ▼
HospitalSubscriptions
        ▲
        │
        │ Many
Hospitals