Based on https://github.com/endel/NativeWebSocket

```shell
# to start the websocket demo, that spams the timestamp every second:
cd NodeServer
npm install
npm run start
```

Right now I want to kill a websocket connection to a server. It runs in a C# task, works fine so far... that is, until I
reload mods, then it seems to get reinstantiated, a second task is spawned, and it either crashed due to the "masked"
old instance. I have not found a place to hook into to disconnect and clean up the event subscriptions on a mod reload,
game session reload.

There are basically three ideas I'm tinkering with that require some kind of network connection, more or less in real
time.

1) A broken mirror. You place it on the floor, then place one of your objects in an adjacent tile near it. The mirror
   will consume that object and spawn an object that has been consumed by another player. Say I place a gun near the
   mirror, it gets consumed. Someone in another game places an armor piece near the mirror, the mirror spawns my weapon
   in their game, but consumes their armor. The next player gets the armor, but has to sacrifice one of his items. Like
   a random trade with another dimension (game).

2) The second one allows you to wire (enable/disable) your vitals to it. It will connect your approximate location,
   vitals and/or heavy hits you take / your deaths and send them to all other players who chose to wire their body.
   Every time a player dies, a technical push towards a websocket or a put towards a web-trigger will collect a "
   memorandum" of the player character data (name, steps, score, level, last messages maybe?, stuff you see on the
   tumbstone screen) for everyone to dig through. But only for players of the same game mode (classic, roleplay, maybe
   daily?).

3) Dark Souls like mentions. When I look at "door", hit space, I want to "guess / ponder". This will get the last 10
   player notes left on the same item. Same goes for things you have equipped, like glowcrust: inspect it, leave a
   comment for other players to give a short max 80-100 ascii message about it. I don't like digging through wikis, but
   I often miss hints from other players. Maybe even vote for answers.

1 + 3 can be done by simple TCP means, which is fine, 2 would work best with websockets.
