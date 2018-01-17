# MozartMusicalDiceCsharp_343
Intermediate Challenge #343 from [r/dailyprogrammer](https://tinyurl.com/ybqu7bvj), "Mozart's Musical Dice".

## Inspiration
[Cosmologicon](https://github.com/cosmologicon) put together a website, [Mozart dice: solution player](http://ufx.space/stuff/mozart-dice/), which would take the challenge's output and play it.
I was inspired to continue this challenge when I discovered how fun it was to generate and listen to the different pieces of music composed from the sample composition. Initially, I tried to write a console application and ran into a limitation imposed by Microsoft on 64 bit computers: console.beep no longer works. Again, this only served to drive me forward.

I had overlooked the website's ability to play sounds; but after a conversation with a close friend of mine the use of the web became painfully obvious.

## New Approach
I had never worked with audio in an application before as my career thus far has been enterprise .Net development. A whole lot of winforms written in VB and C#, intranet sites using various versions of ASP.NET MVC, and more.
Faced with this new challenge I turned to cosmologicon's website for guidance. I was introduced to to the [Web Audio API](https://developer.mozilla.org/en-US/docs/Web/API/Web_Audio_API) through his js code and began analyzing why he wrote it the way he did.
I chose to use ASP.NET MVC because it's been a since I programmed in it; I find it rather fun. I have slowly started to recreate his code using tests.

## What I've Been Using to Test With
[Jim Counts](https://github.com/jamesrcounts) has a nice [blog series](https://ihadthisideaonce.com/2012/05/15/approvaltests-and-mvc-views-getting-started/) on unit testing ASP.NET MVC with the [ASP Approval Test](https://github.com/approvals/Approvals.Net.Asp) framework, which is also available as a  [nuget](https://www.nuget.org/packages/ApprovalTests.Asp/) package.
As for the javascript code, QUnit is the recommended method for testing it. I didn't like the look and feel of QUnit, so I instead chose to use mocha as my testing framework. Since I don't have a "module.exports" available to me in my MVC app, I created an object to contain my functions and added said object to the global "this" so both my app and the testing app can utilize the functions.
