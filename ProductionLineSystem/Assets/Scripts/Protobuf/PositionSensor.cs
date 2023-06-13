// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: PositionSensor.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
/// <summary>Holder for reflection information generated from PositionSensor.proto</summary>
public static partial class PositionSensorReflection {

  #region Descriptor
  /// <summary>File descriptor for PositionSensor.proto</summary>
  public static pbr::FileDescriptor Descriptor {
    get { return descriptor; }
  }
  private static pbr::FileDescriptor descriptor;

  static PositionSensorReflection() {
    byte[] descriptorData = global::System.Convert.FromBase64String(
        string.Concat(
          "ChRQb3NpdGlvblNlbnNvci5wcm90byJ3ChNQb3NpdGlvblNlbnNvcl9EYXRh",
          "Eg8KAmlkGAEgASgFSACIAQESFgoJaXNBY3RpdmVkGAIgASgISAGIAQESFQoI",
          "ZGF0YVRpbWUYAyABKAlIAogBAUIFCgNfaWRCDAoKX2lzQWN0aXZlZEILCglf",
          "ZGF0YVRpbWViBnByb3RvMw=="));
    descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
        new pbr::FileDescriptor[] { },
        new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
          new pbr::GeneratedClrTypeInfo(typeof(global::PositionSensor_Data), global::PositionSensor_Data.Parser, new[]{ "Id", "IsActived", "DataTime" }, new[]{ "Id", "IsActived", "DataTime" }, null, null, null)
        }));
  }
  #endregion

}
#region Messages
public sealed partial class PositionSensor_Data : pb::IMessage<PositionSensor_Data>
#if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    , pb::IBufferMessage
#endif
{
  private static readonly pb::MessageParser<PositionSensor_Data> _parser = new pb::MessageParser<PositionSensor_Data>(() => new PositionSensor_Data());
  private pb::UnknownFieldSet _unknownFields;
  private int _hasBits0;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public static pb::MessageParser<PositionSensor_Data> Parser { get { return _parser; } }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public static pbr::MessageDescriptor Descriptor {
    get { return global::PositionSensorReflection.Descriptor.MessageTypes[0]; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  pbr::MessageDescriptor pb::IMessage.Descriptor {
    get { return Descriptor; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public PositionSensor_Data() {
    OnConstruction();
  }

  partial void OnConstruction();

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public PositionSensor_Data(PositionSensor_Data other) : this() {
    _hasBits0 = other._hasBits0;
    id_ = other.id_;
    isActived_ = other.isActived_;
    dataTime_ = other.dataTime_;
    _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public PositionSensor_Data Clone() {
    return new PositionSensor_Data(this);
  }

  /// <summary>Field number for the "id" field.</summary>
  public const int IdFieldNumber = 1;
  private readonly static int IdDefaultValue = 0;

  private int id_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public int Id {
    get { if ((_hasBits0 & 1) != 0) { return id_; } else { return IdDefaultValue; } }
    set {
      _hasBits0 |= 1;
      id_ = value;
    }
  }
  /// <summary>Gets whether the "id" field is set</summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public bool HasId {
    get { return (_hasBits0 & 1) != 0; }
  }
  /// <summary>Clears the value of the "id" field</summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void ClearId() {
    _hasBits0 &= ~1;
  }

  /// <summary>Field number for the "isActived" field.</summary>
  public const int IsActivedFieldNumber = 2;
  private readonly static bool IsActivedDefaultValue = false;

  private bool isActived_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public bool IsActived {
    get { if ((_hasBits0 & 2) != 0) { return isActived_; } else { return IsActivedDefaultValue; } }
    set {
      _hasBits0 |= 2;
      isActived_ = value;
    }
  }
  /// <summary>Gets whether the "isActived" field is set</summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public bool HasIsActived {
    get { return (_hasBits0 & 2) != 0; }
  }
  /// <summary>Clears the value of the "isActived" field</summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void ClearIsActived() {
    _hasBits0 &= ~2;
  }

  /// <summary>Field number for the "dataTime" field.</summary>
  public const int DataTimeFieldNumber = 3;
  private readonly static string DataTimeDefaultValue = "";

  private string dataTime_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public string DataTime {
    get { return dataTime_ ?? DataTimeDefaultValue; }
    set {
      dataTime_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
    }
  }
  /// <summary>Gets whether the "dataTime" field is set</summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public bool HasDataTime {
    get { return dataTime_ != null; }
  }
  /// <summary>Clears the value of the "dataTime" field</summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void ClearDataTime() {
    dataTime_ = null;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public override bool Equals(object other) {
    return Equals(other as PositionSensor_Data);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public bool Equals(PositionSensor_Data other) {
    if (ReferenceEquals(other, null)) {
      return false;
    }
    if (ReferenceEquals(other, this)) {
      return true;
    }
    if (Id != other.Id) return false;
    if (IsActived != other.IsActived) return false;
    if (DataTime != other.DataTime) return false;
    return Equals(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public override int GetHashCode() {
    int hash = 1;
    if (HasId) hash ^= Id.GetHashCode();
    if (HasIsActived) hash ^= IsActived.GetHashCode();
    if (HasDataTime) hash ^= DataTime.GetHashCode();
    if (_unknownFields != null) {
      hash ^= _unknownFields.GetHashCode();
    }
    return hash;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public override string ToString() {
    return pb::JsonFormatter.ToDiagnosticString(this);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void WriteTo(pb::CodedOutputStream output) {
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    output.WriteRawMessage(this);
  #else
    if (HasId) {
      output.WriteRawTag(8);
      output.WriteInt32(Id);
    }
    if (HasIsActived) {
      output.WriteRawTag(16);
      output.WriteBool(IsActived);
    }
    if (HasDataTime) {
      output.WriteRawTag(26);
      output.WriteString(DataTime);
    }
    if (_unknownFields != null) {
      _unknownFields.WriteTo(output);
    }
  #endif
  }

  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
    if (HasId) {
      output.WriteRawTag(8);
      output.WriteInt32(Id);
    }
    if (HasIsActived) {
      output.WriteRawTag(16);
      output.WriteBool(IsActived);
    }
    if (HasDataTime) {
      output.WriteRawTag(26);
      output.WriteString(DataTime);
    }
    if (_unknownFields != null) {
      _unknownFields.WriteTo(ref output);
    }
  }
  #endif

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public int CalculateSize() {
    int size = 0;
    if (HasId) {
      size += 1 + pb::CodedOutputStream.ComputeInt32Size(Id);
    }
    if (HasIsActived) {
      size += 1 + 1;
    }
    if (HasDataTime) {
      size += 1 + pb::CodedOutputStream.ComputeStringSize(DataTime);
    }
    if (_unknownFields != null) {
      size += _unknownFields.CalculateSize();
    }
    return size;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void MergeFrom(PositionSensor_Data other) {
    if (other == null) {
      return;
    }
    if (other.HasId) {
      Id = other.Id;
    }
    if (other.HasIsActived) {
      IsActived = other.IsActived;
    }
    if (other.HasDataTime) {
      DataTime = other.DataTime;
    }
    _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void MergeFrom(pb::CodedInputStream input) {
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    input.ReadRawMessage(this);
  #else
    uint tag;
    while ((tag = input.ReadTag()) != 0) {
      switch(tag) {
        default:
          _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
          break;
        case 8: {
          Id = input.ReadInt32();
          break;
        }
        case 16: {
          IsActived = input.ReadBool();
          break;
        }
        case 26: {
          DataTime = input.ReadString();
          break;
        }
      }
    }
  #endif
  }

  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
    uint tag;
    while ((tag = input.ReadTag()) != 0) {
      switch(tag) {
        default:
          _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
          break;
        case 8: {
          Id = input.ReadInt32();
          break;
        }
        case 16: {
          IsActived = input.ReadBool();
          break;
        }
        case 26: {
          DataTime = input.ReadString();
          break;
        }
      }
    }
  }
  #endif

}

#endregion


#endregion Designer generated code
