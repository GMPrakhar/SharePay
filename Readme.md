# SharePay

SharePay is a multi-user application designed to manage group expenses and transactions. The application consists of an API backend and a platform independent UI frontend. 
The aim is providing a Free-and-Open-Source platform to solve the most basic problem of the society, division of currency, and so we strive to keep it away from ads and premium only services.

## Table of Contents

- [Features](#features)
- [Technologies](#technologies)
- [Setup](#setup)
- [Usage](#usage)
- [API Endpoints](#api-endpoints)
- [Contributing](#contributing)
- [License](#license)

## Features

- Manage groups and users.
- Add and view transactions within groups.
- View consolidated balances for group transactions.
- Dockerized for easy deployment.

## Technologies

- .NET 6
- ASP.NET Core
- MAUI
- SQL Server
- Docker

## Setup

### Prerequisites

- Docker
- Docker Compose

### Steps

1. **Clone the repository:**

    ```sh
    git clone https://github.com/GMPrakhar/SharePay.git
    cd SharePay
    ```

2. **Build and run the Docker containers:**

    ```sh
    docker-compose build
    docker-compose up
    ```

This will build and run the Docker containers for the API, UI, and database.

## Usage

Once the containers are up and running, you can access the application:

- **API:** `http://localhost:5000`
- **UI:** `http://localhost:5001`

## API Endpoints

### Group Endpoints

- `POST /v1/groups` - Create a new group.
- `GET /v1/user/{userId}/groups` - List groups for a user.
- `GET /v1/groups/{groupId}/users` - Get users for a group.
- `GET /v1/groups/{groupId}/transactions` - Get transactions for a group.
- `GET /v1/groups/{groupId}/transactions/consolidated` - Get consolidated transactions for a group.

### Transaction Endpoints

- `POST /v1/groups/{groupId}/transactions` - Add a new transaction to a group.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any improvements or bug fixes.

## License

This project is licensed under the MIT License. See the LICENSE file for details.