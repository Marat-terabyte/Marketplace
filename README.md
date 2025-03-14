# Marketplace
Marketplace is a modern platform for interatcion between sellers and consumers, developed using microservice architecture. The project provides high scalability, flexibility, fault-tolerance, which makes it an ideal solution for creating online marketplaces.

## Stack of technologies
- C#
- TypeScript
- React
- ASP .Net Core
- Redis
- MongoDB
- Elasticsearch
- RabbitMQ
- SQLite
- Docker

## Installation and running
1. Clone the repo  
  ```sh
  git clone https://github.com/Marat-terabyte/Marketplace.git
  ```
2. Change directory
  ```sh
  cd Marketplace
  ```
3. Build docker images of Marketplace
  ```sh
  docker compose build
  ```
4. Run docker containers
  ```sh
  docker compose up
  ```
5. Open http://localhost:81 in your browser

## Architecture
The project uses a microservice architecture, each service implements a separate business logic. All services communicate with each other using HTTP and RabbitMQ protocol. The client interacts with the services through an API gateway written in Ocelot .Net. Such a solution allows the project to be sacalizable and flexible in expansion  
  
**System schematic** - <a href="https://miro.com/app/board/uXjVLrtizx4=/?share_link_id=946458962597">click</a>

## The most important microservices
1. **Identity Service**: user management (registration, authorization).
2. **Product Service**: product management (adding, deleting, editing).
3. **Search Service**: search products by query using Elasticsearch.
4. **Notification Service**: sening notifications to users using SignalR.

## License
Distributed under the MIT License.