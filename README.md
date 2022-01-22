# com.noz.tweening

Light weight and effecient tweening library for Unity that provides easy support to tween most value types on any object.

## Installation

<https://docs.unity3d.com/Packages/com.unity.package-manager-ui@2.0/manual/index.html>

```json
{
    "dependencies": {
        "com.noz.tweening": "https://github.com/nozgames/com.noz.tweening.git#main"
    }
}
```

## Examples

Builder interface to easily chain options together 

```
target.TweenFloat("property", 100.0f).Duration(2.0f).EaseInCubic().EaseOutSine().Loop(4).Play();
```

Tween the color of any graphic

```
GetComponent<Graphic>().TweenColor(Color.red).Play();
```

Tween a float parameter of a material

```
material.TweenFloat ("_BorderSize", 10.0f).Play();
```

## Documentation

Coming soon
