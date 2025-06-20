# Stake Limit Service API

---

## Features

- **Update configuration for a device**
- **Get all devices (with optional query)**
- **Create and evaluate a ticket for a device**
- **Dockerized with PostgreSQL**
- **Swagger UI for API documentation**

---

## Routes

### Device Routes

#### `PUT /api/devices/{deviceId}/config`
-  **UpsertConfig**: Updates the configuration for the given device if it exists, or creates a new device with the provided configuration if it does not.

#### `GET /api/devices`
- **GetAllDevices**: Retrieves all devices (optionally filtered by query parameters).

---

### Ticket Routes

#### `POST /api/tickets`
- **CreateTicket**: Evaluates and creates a ticket for the given device.

---

## Technologies

- **.NET 8 (C#)**
- **PostgreSQL**
- **Swagger**
- **Docker & Docker Compose**
- **xUnit** (for testing)

---

## Running the software

To start all app-dependent containers simultaneously, you must have Docker Compose installed.

1. Navigate to the root folder containing `docker-compose.yml`.
2. Start the stack with:

   ```sh
   docker-compose up --build
   ```

3. The API will be available at:  
   [http://localhost:5000](http://localhost:5000)

4. Swagger UI is available at:  
   [http://localhost:5000/swagger](http://localhost:5000/swagger)

5. Environment variables are set in `docker-compose.yml`.  
   You can customize them as needed.

---

## Testing the software

To run tests (if using xUnit):

```sh
docker-compose run api dotnet test
```

---

**Note:**  
- Make sure ports in the documentation match your `docker-compose.yml` mapping (`5000:8080`).
- Update endpoints and technologies if your implementation differs.
