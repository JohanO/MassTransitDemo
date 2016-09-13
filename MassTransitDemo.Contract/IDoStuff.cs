// Copyright © Tetra Pak 2015 All rights reserved. No part of this
// program may be reproduced, stored in a retrieval system, or
// transmitted, in any form or by any means, without the prior
// permission, in writing, of Tetra Pak.

using System;

namespace MassTransitDemo.Contract
{
    public interface IDoStuff : ICommand
    {
        int StuffNumber { get; }
        double StuffSize { get; }
        DateTimeOffset Timestamp { get; }
    }
}