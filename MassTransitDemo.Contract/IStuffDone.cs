// Copyright © Tetra Pak 2015 All rights reserved. No part of this
// program may be reproduced, stored in a retrieval system, or
// transmitted, in any form or by any means, without the prior
// permission, in writing, of Tetra Pak.
namespace MassTransitDemo.Contract
{
    public interface IStuffDone : IEvent
    {
        int StuffNumber { get; }
        double StuffSize { get; }
        double Timestamp { get; }
    }
}