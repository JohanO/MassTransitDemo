# MassTransitDemo
This code demonstrates [CQRS principles](http://udidahan.com/2009/12/09/clarified-cqrs/) using [MassTransit](http://masstransit-project.com/) 
and [RabbitMQ](http://www.rabbitmq.com/).

## Parts

### Overview
![Application parts](Overview.png)

The system consist of four apps that communicate by sending messages via RabbitMQ.

### Contract
The contract defines the messages the apps use for communication.

There are three kinds of messages:
* Command: Tell the receiver to do something.
* Event: Notify everyone that something has happened.
* Query: Ask for specific information

