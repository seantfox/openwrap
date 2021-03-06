﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OpenWrap.Commands;
using OpenWrap.Commands.Wrap;
using OpenWrap.Testing;

namespace OpenWrap.Tests.Commands
{
    [TestFixture]
    public class command_documentation
    {
        [Datapoints]
        public ICommandDescriptor[] commands =
                GetCommands();

        static ICommandDescriptor[] GetCommands()
        {
            return (
                           from type in typeof(AddWrapCommand).Assembly.GetExportedTypes()
                           where type.IsAbstract == false && type.IsGenericTypeDefinition == false &&
                                 type.GetInterface(typeof(ICommand).Name) != null
                           select (ICommandDescriptor)new AttributeBasedCommandDescriptor(type)
                   ).ToArray();
        }

        [Theory]
        public void command_has_documentation(ICommandDescriptor command)
        {
            command.Description.ShouldNotBeNullOrEmpty(string.Format("command '{0}-{1}' doesn't have a description",command.Verb, command.Noun));
            foreach (var commandInput in command.Inputs)
                commandInput.Value.Description.ShouldNotBeNullOrEmpty(string.Format("command input '{0}' for command '{1}-{2}' doesn't have a description", commandInput.Value.Name,command.Verb, command.Noun));
        }
    }
}
