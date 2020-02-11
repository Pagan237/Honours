black = shoot
magenta = retreat
blue = move
cyan = reload
green = heal

States:
0: health = high, ammo = low, !Insight, hit = yes
1: health = low, ammo = high, !inSight, hit = yes
2: health = high, ammo = high, !inSight, hit = yes
3: health = low, ammo = low, inSight, hit = yes
4: health = high, ammo = low, inSight, Hit = yes
5: health = low, ammo = high, inSight, hit = yes
6: health = high, ammo = high, inSight, hit = yes
7: health = low, ammo = low, !inSight, hit = not
8: health = high, ammo = low, !inSight, Hit = not
9: health = low, ammo = high, !inSight, hit = not
10: health = high, ammo = high, !inSight, Hit = not
11: health = low, ammo = low, inSight, hit = not
12: health = high, ammo = low, inSight, Hit = not
13: health = low, ammo = high, inSight, hit = not
14: health = high, ammo = high, inSight, hit = not