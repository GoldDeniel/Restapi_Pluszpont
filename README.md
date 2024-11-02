# MongoDB
```
docker run -d --rm --name mongodb-api -e MONGO_INITDB_ROOT_USERNAME=root -e MONGO_INITDB_ROOT_PASSWORD=root -e MONGO_INITDB_DATABASE=BookStore -v /tmp/mongo-data:/data/db mongo
```
