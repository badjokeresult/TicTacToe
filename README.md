# Web API for Tic-Tac-Toe game

## This API uses to manage data of the two entities: players and games

## Game entity has both players Ids from player entity

### Player entity:

- Address: api/v1/player
- API-methods:
  - get/{id} - receives a player object by id
    - go to url "api/v1/player/get/{id}"
  - get-all - receives all players' objects
    - go to url "api/v1/player/get-all"
  - create - adds a new player object
    - send a POST-request by the url "api/v1/player/create" with a JSON like `{"Name": "first_player", "Cells": [[0,0],[1,1],[2,2]]}`
  - update - replaces an old player object by new one
    - send a PUT-request by the url "api/v1/player/update" with a JSON like `{"Id": <player_id>, "Name": "first_player", "Cells": [[0,0],[1,1],[2,2]]}`
  - delete/{id} - removes a player object by id
    - send a DELETE-request by the url "api/v1/player/delete/{id}"

### Game entity:

- Address: api/v1/game
- API-methods:
  - get/{id} - receives a game object by id
    - go to a url "api/v1/game/get/{id}":
  - get-all - receives all games' objects
    - go to a url "api/v1/game/get-all"
  - create - adds a new game object
    - send a POST-request with a JSON like: `"{"FirstPlayer": {"Name": "first_player", "Cells": [[0,0],[1,1],[2,2]]}, "SecondPlayer": {"Name": "second_player", "Cells": [[0,1][1,2][2,0]]}}"`
  - update - replaces an old game object by a new one
    - send a PUT-request with a JSON like: `"{"Id": <game_id, "FirstPlayer": {"Name": "first_player", "Cells": [[0,0],[1,1],[2,2]]}, "SecondPlayer": {"Name": "second_player", "Cells": [[0,1][1,2][2,0]]}}"`
  - delete/{id} - removes a player object by id
    - send a DELETE-request by the url "api/v1/game/delete/{id}"
  
### This API also checks the correctness of the given params like:
- Existence of players with given ids in a game record
- The situation when both players' ids are equal in one game record
- The situation when both players' cells are invalid (cells' values interception)

## Here is a little non-professional structure of this API
![img.png](img.png)