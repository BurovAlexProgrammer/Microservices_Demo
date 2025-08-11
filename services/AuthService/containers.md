    📥 Скачивание образа:
    bash
    docker pull postgres:15

    ▶️ Запуск контейнера для userdb:
    bash
    docker run --name user-postgres \
    -e POSTGRES_DB=userdb \
    -e POSTGRES_USER=useruser \
    -e POSTGRES_PASSWORD=userpass \
    -p 5434:5432 \
    -v user_pgdata:/var/lib/postgresql/data \
    -d postgres:15

    ▶️ Запуск контейнера для chatdb:
    bash
    docker run --name chat-postgres \
    -e POSTGRES_DB=chatdb \
    -e POSTGRES_USER=chatuser \
    -e POSTGRES_PASSWORD=chatpass \
    -p 5433:5432 \
    -v chat_pgdata:/var/lib/postgresql/data \
    -d postgres:15
    🐳 Kafka + Zookeeper
    Kafka требует Zookeeper, поэтому два контейнера:
    
    📥 Скачивание образов:
    bash
    docker pull confluentinc/cp-zookeeper:7.5.0
    docker pull confluentinc/cp-kafka:7.5.0

    ▶️ Zookeeper:
    bash
    docker run --name zookeeper \
    -e ZOOKEEPER_CLIENT_PORT=2181 \
    -p 2181:2181 \
    -d confluentinc/cp-zookeeper:7.5.0

    ▶️ Kafka:
    bash
    docker run --name kafka \
    -e KAFKA_BROKER_ID=1 \
    -e KAFKA_ZOOKEEPER_CONNECT=zookeeper:2181 \
    -e KAFKA_LISTENERS=PLAINTEXT://:9092 \
    -e KAFKA_ADVERTISED_LISTENERS=PLAINTEXT://localhost:9092 \
    -e KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR=1 \
    --link zookeeper \
    -p 9092:9092 \
    -d confluentinc/cp-kafka:7.5.0
    ⚠️ Kafka использует hostname localhost, чтобы ты мог подключаться к нему извне. Если планируешь запускать микросервисы в других контейнерах — придётся сменить localhost на kafka или host.docker.internal.
    
    📦 Управление
    Проверка контейнеров:
    bash
    docker ps
    Остановка:
    bash
    docker stop user-postgres chat-postgres zookeeper kafka
    Удаление:
    bash
    docker rm -f user-postgres chat-postgres zookeeper kafka

    🔧 Проверка подключения
    Можешь использовать, например, pgAdmin, DBeaver или psql:
    
    localhost:5434, БД: userdb, логин: useruser, пароль: userpass
    
    localhost:5433, БД: chatdb, логин: chatuser, пароль: chatpass
    
    Kafka можешь тестировать через консоль или с помощью kafkacat, kcat, или встроенного клиента в .NET позже.