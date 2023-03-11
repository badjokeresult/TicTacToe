# Web API for Tic-Tac-Toe game

## This API uses to manage data of the two entities: players and games

## Game entity has both players Ids from player entity

### Player entity:

- Address: api/v1/player
- API-methods:
  - /get/{id} - receives a player object by id
  - /get - receives all players' objects
  - /create - adds a new player object
  - /update - replaces an old player object by new one
  - /delete/{id} - removes a player object by id

### Game entity:

- Address: api/v1/game
- API-methods:
  - /get/{id} - receives a game object by id
  - /get - receives all games' objects
  - /create - adds a new game object
  - /update - replaces an old game object by a new one
  - /delete/{id} - removes a player object by id
  
### This API also checks the correctness of the given params like:
- Existence of players with given ids in a game record
- The situation when both players' ids are equal in one game record
- The situation when both players' cells are invalid (cells' values interception)

## Here is a little non-professional structure of this API
![img.png](img.png)