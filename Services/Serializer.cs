﻿using System;
using System.IO;
using NoviSad.SokoBot.Data.Entities;
using NoviSad.SokoBot.Tools;

namespace NoviSad.SokoBot.Services;

public static class Serializer {
    private static T? ReadT<T>(BinaryReader? reader, Func<BinaryReader, T> readValue)
        where T : struct {
        return ReadU(reader, readValue, x => x);
    }

    private static U? ReadU<T, U>(BinaryReader? reader, Func<BinaryReader, T> readValue, Func<T, U> convert)
        where T : struct
        where U : struct {
        var hasValue = reader.ReadBoolean();
        var value = readValue(reader);

        return hasValue ? convert(value) : default(U?);
    }

    public static string Serialize(RequestContext context) {
        using var stream = new MemoryStream(64);
        using (var writer = new BinaryWriter(stream)) {
            writer.Write((byte)137);

            writer.Write(context.Cancel);

            writer.Write(context.Spectate);

            writer.Write(context.Direction.HasValue);
            writer.Write((byte)(context.Direction ?? 0));

            writer.Write(context.SearchStart.HasValue);
            writer.Write((context.SearchStart ?? default).UtcTicks);

            writer.Write(context.SearchEnd.HasValue);
            writer.Write((context.SearchEnd ?? default).UtcTicks);

            writer.Write(context.TrainNumber.HasValue);
            writer.Write(context.TrainNumber ?? default);

            writer.Write(context.DepartureTime.HasValue);
            writer.Write((context.DepartureTime ?? default).UtcTicks);

            writer.Write(context.Leave);
        }

        return Convert.ToBase64String(stream.ToArray());
    }

    public static RequestContext? DeserializeRequestContext(string? context) {
        if (string.IsNullOrEmpty(context))
            return null;

        using var stream = new MemoryStream(Convert.FromBase64String(context));
        using var reader = new BinaryReader(stream);
        if (reader.ReadByte() != 137)
            return null;

        var cancel = reader.ReadBoolean();
        var spectate = reader.ReadBoolean();
        var direction = ReadU(reader, x => x.ReadByte(), x => (TrainDirection)x);
        var searchStart = ReadU(reader, x => x.ReadInt64(), x => new DateTimeOffset(x, TimeSpan.Zero));
        var searchEnd = ReadU(reader, x => x.ReadInt64(), x => new DateTimeOffset(x, TimeSpan.Zero));
        var trainNumber = ReadT(reader, x => x.ReadInt32());
        var departureTime = ReadU(reader, x => x.ReadInt64(), x => new DateTimeOffset(x, TimeSpan.Zero));
        var leave = reader.ReadBoolean();

        return new RequestContext(cancel, spectate, direction, searchStart, searchEnd, trainNumber, departureTime, leave);
    }

    public static string Serialize(TrainQuery context) {
        using var stream = new MemoryStream(64);
        using (var writer = new BinaryWriter(stream)) {
            writer.Write((byte)139);

            writer.Write(context.TrainNumber);
            writer.Write(context.DepartureTime.UtcTicks);
            writer.Write(context.Leave);
        }

        return Convert.ToBase64String(stream.ToArray());
    }

    public static TrainQuery? DeserializeTrainQuery(string? context) {
        if (string.IsNullOrEmpty(context))
            return null;

        using var stream = new MemoryStream(Convert.FromBase64String(context));
        using var reader = new BinaryReader(stream);
        if (reader.ReadByte() != 139)
            return null;

        var trainNumber = reader.ReadInt32();
        var departureTime = new DateTimeOffset(reader.ReadInt64(), TimeSpan.Zero);
        var leave = reader.ReadBoolean();

        return new TrainQuery(trainNumber, departureTime) { Leave = leave };
    }
}
