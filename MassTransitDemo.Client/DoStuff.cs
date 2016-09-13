// Copyright © Tetra Pak 2015 All rights reserved. No part of this
// program may be reproduced, stored in a retrieval system, or
// transmitted, in any form or by any means, without the prior
// permission, in writing, of Tetra Pak.

using System;

using MassTransitDemo.Contract;

namespace MassTransitDemo.Client
{
    public class DoStuff : Command, IDoStuff
    {
        public DoStuff(int number, double size)
        {
            StuffNumber = number;
            StuffSize = size;
            Timestamp = DateTimeOffset.Now;
        }

        public int StuffNumber { get; }

        public double StuffSize { get; }

        public DateTimeOffset Timestamp { get; }
    }
}