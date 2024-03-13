# flight-search-aggregator

проект состоит из 3 частей
2 сервиса по авиабилетам

1 один сервис агрегатор


# FlyZen
запуск  ```dotnet run -lp https```


## получение тикетов
```curl
curl --location 'https://localhost:7191/tickets?from=2024-03-01&to=2024-03-03' \
--header 'accept: */*' \
--header 'apiToken: g#$%Hw4%H;k345gjw'\''4ph'\'''
```

## бронирование

```curl
curl --location --request POST 'https://localhost:7191/tickets/222-33-5792/order' \
--header 'accept: */*' \
--header 'apiToken: g#$%Hw4%H;k345gjw'\''4ph'\'''
```

# ZotFlightService

запуск  ```dotnet run -lp https```


## получение тикетов
```curl
curl --location 'https://localhost:7211/flights?from=2024-03-01&to=2024-03-03' \
--header 'accept: */*' \
--header 'apiToken: g#$%Hw4%H;k345gjw'\''4ph'\'''
```

## бронирование

```curl
curl --location --request POST 'https://localhost:7211/flight/412-67-9971/order' \
--header 'accept: */*' \
--header 'apiToken: g#$%Hw4%H;k345gjw'\''4ph'\'''
```


