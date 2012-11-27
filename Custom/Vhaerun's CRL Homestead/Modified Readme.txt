Modified Scripts:

In the Modified directory (the same one this readme should be found in) are a
number of scripts that have been changed to include additional functionality or
changes that are related to the food/cooking system.


Beverage.cs - A modified Beverage script, the only additions are messages for
drinking, as well as a cap at 20 thirst.

CreateFood.cs - An enhanced Create Food spell that draws upon the new food items
and creates three items instead of one.

DefCooking.cs - Adds a menu for some of the new cooking items. Slowly adding
more to this, right now it has the specialty cake mixes, specialty cakes, and a
"single barbecue" menu for cooking things one at a time rather than a whole
stack at once.

FoodDecay.cs - Includes hunger/thirst warnings at 5 or less. Includes a minor HP
penalty though really that should go in a modified regenrate script.
Includes "Staff Immunity" variable and "Keep Alive" variable.
StaffImmune - Staff hunger/thirst will not decay below 10, if true.
KeepAlive - Extreme Hunger/Thirst will not kill player, if true.

TasteID.cs - Tasting StaticTarget or AddonComponent mushrooms may give you some
edible mushrooms!  This is buggy in that seasonal mushrooms (fall mushrooms,
when the season is autumn) cannot be targetted for some reason. Also you can
taste the same patch over and over again, ideally this should be a resource bank
or something that is depleted and then renews over time.