# Test.It.While.Hosting.Your.Service
A Testing framework that hosts your service during test execution and let you write BDD like test specifications. 

The old deprecated windows service test project can be found here: https://github.com/Fresa/Test.It.While.Hosting.Your.Windows.Service

[![Build status](https://ci.appveyor.com/api/projects/status/1wt1duwbn6kh3yn7?svg=true)](https://ci.appveyor.com/project/Fresa/test-it-while-hosting-your-service)

[![Build history](https://buildstats.info/appveyor/chart/Fresa/test-it-while-hosting-your-service)](https://ci.appveyor.com/project/Fresa/test-it-while-hosting-your-service/history)

## Why?
This library helps you simplify writing integration tests for your service. It will bootstrap, start and host your service in memory while giving you handles to interact with the service making it possible to mock integration points to isolate the service during testing. This way you are no longer dependent on 3rd party installations when you run your application, and you no longer need to do tricky cleanup sessions before and after your tests, everything is done in memory and erased as soon as the test finishes.

## What's an Integration Test?
Good question! When it comes do different testing approches and terms, they tend to differ among us developers. In this context an integration test is a test that let you probe your 3rd party integration points while the application is running.

**Example:**
You have a service that receives commands from a message queue, aggregates events from a event store, applies domain logics, generate events which are stored in the event store and sent out on the message queue. The process looks something like this:
1. Receive command from message queue
2. Load a domain object by fetching events from an event store.
3. Apply domain logic based on the state of the domain object and the command being executed and generate new events.
4. Store events in the event store.
5. Send the new events onto the message queue.

So here we have two integration points:
1. The event store (let's say PostgreSQL)
2. The Message Queue (let's say RabbitMQ)

During a continues integration process, your build server probably runs tests. If it were to run an integration test, it would need to host the service and have control over a PostgreSQL and a RabbitMQ instance. These would be hard to control in an orderly fashion, specially during parallel testing. 

In comes Test.It.While.Hosting.Your.Service to save the day!

## Download
https://www.nuget.org/packages/Test.It.While.Hosting.Your.Service/

[![NuGet Badge](https://buildstats.info/nuget/Test.It.While.Hosting.Your.Service)](https://www.nuget.org/packages/Test.It.While.Hosting.Your.Service/)

## Release Notes
**1.1.0** Changed the definition and name of the IServiceConfiguration to IServiceHostStarter

## Getting Started
tl;dr:
Runnable example available here: https://github.com/Fresa/Test.It.While.Hosting.Your.Service/blob/master/tests/Test.It.While.Hosting.Your.Service.Tests/When_testing_a_service.cs

### Setting Up Your First Test
It all begins by you creating a test class which will inherit `ServiceSpecification`. This specification defines how to configure, build, start and host your service and at the same time create a communication channel between your test and the hosted service and control the whole test process. All without forcing you into a certain test framework, ofcourse.

The `ServiceSpecification` requires an implementation of the `IServiceHostStarter` as generic parameter. You may roll your own configuration, or you can use the `DefaultServiceHostStarter` which will work in most cases. If you go for the latter it will need an application builder as generic parameter, i.e. you need to implement `IServiceBuilder`. `IServiceBuilder` will tell the hosting framework how to start your application. 

You need to specify a test configurer in your implementation of the `IServiceBuilder`. The test configurer will let you reconfigure your application during startup so you can overwrite the 3rd party integration client registration for example. You will need an implementation of the `IServiceContainer` interface to achive this. I highly recommend using an IOC/DI container (my favorite is SimpleInjector, https://simpleinjector.org/), but you are free to roll your own service container, see [`SimpleServiceContainer`](https://github.com/Fresa/Test.It.While.Hosting.Your.Service/blob/master/tests/Test.It.While.Hosting.Your.Service.Tests/SimpleServiceContainer.cs).

Now, in your test, you will have a `Client` property available where you can control the service. It only exposes a `Disconnect` method, which you at most cases do not need to use, because the `ServiceSpecification` will handle shutdown when your test has finished.

### BDD
`ServiceSpecification` uses a BDD style for arranging your test. You can read more about BDD here: https://en.wikipedia.org/wiki/Behavior-driven_development

#### Given
`ServiceSpecification` exposes a method called `Given` which is overridable. This is where you set up your test. It exposes the `IServiceContainer` of your application, where you can override behaviour like the client implementation to your 3rd party applications (see Best Practices).

#### When
`ServiceSpecification` also exposes a method called `When`. This is where you execute some method exposed by any of your integration points for example.

#### Then
This is your test method. This is where you assert what ever you expect to happen.

### Best Practices
When reconfiguring your application during startup, please be advised that doing to much reconfiguring will heavy alter your application behaviour and you might no longer test any relevant functionality of the service. Keep the reconfiguration to a minimum to be sure to test as much as possible of the soon to be live functionality. Try to reconfigure your 3rd party dependecy systems as close to the network level as possible to assure you keep the reconfiguration to a minimum.
