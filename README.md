# Microservices_Demo

Настроены сервисы для работы кафки с одним нодом
<img width="2072" height="390" alt="image" src="https://github.com/user-attachments/assets/58f58870-020f-48e3-82f7-b92e14f60988" />

Настроен мониторинг топиков Kafkadrop http://localhost:9000/
<img width="2122" height="1486" alt="{3FF1EA85-48EE-46EC-8119-44B0BE219C72}" src="https://github.com/user-attachments/assets/55fd6b59-1c29-4a5f-ba00-32a179365e9a" />

nginx http://localhost:80/

docker compose -f ./services/docker-compose.yml up -d

docker build -t auth-service ./services/auth-service
docker run -it --name auth-service --network mms_network -p 3001:3001 auth-service