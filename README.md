# MongoDB
```
docker run -d --rm --name mongodb-api -e MONGO_INITDB_ROOT_USERNAME=root -e MONGO_INITDB_ROOT_PASSWORD=root -e MONGO_INITDB_DATABASE=BookStore -v /tmp/mongo-data:/data/db mongo
```

# JavaScript

A javascript részben a tutorial kódját módosítanom kellett, mert nem működőtt a site.js.
A probléma ott volt, hogy az átadott property nevek nem egyeztek. 

```
item.id => item.Id
item.name => item.Name
item.isComplete => item.IsComplete
```
