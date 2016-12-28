#ifndef _GAP_ADVERTISING_DATA_BUILDER_H_
#define _GAP_ADVERTISING_DATA_BUILDER_H_

enum GAPDataType_t
{
  GAP_DATATYPE_FLAGS = 0x01,
  GAP_DATATYPE_INCOMPLETE_LIST_OF_16_BIT_SERVICE_CLASS_UUIDS = 0x02,
  GAP_DATATYPE_COMPLETE_LIST_OF_16_BIT_SERVICE_CLASS_UUIDS = 0x03,
  GAP_DATATYPE_INCOMPLETE_LIST_OF_32_BIT_SERVICE_CLASS_UUIDS = 0x04,
  GAP_DATATYPE_COMPLETE_LIST_OF_32_BIT_SERVICE_CLASS_UUIDS = 0x05,
  GAP_DATATYPE_INCOMPLETE_LIST_OF_128_BIT_SERVICE_CLASS_UUIDS = 0x06,
  GAP_DATATYPE_COMPLETE_LIST_OF_128_BIT_SERVICE_CLASS_UUIDS = 0x07,
  GAP_DATATYPE_SHORTENED_LOCAL_NAME = 0x08,
  GAP_DATATYPE_COMPLETE_LOCAL_NAME = 0x09,
  GAP_DATATYPE_TX_POWER_LEVEL = 0x0A,
  GAP_DATATYPE_CLASS_OF_DEVICE = 0x0D,
  GAP_DATATYPE_SIMPLE_PAIRING_HASH_C = 0x0E,
  GAP_DATATYPE_SIMPLE_PAIRING_HASH_C_192 = 0x0E,
  GAP_DATATYPE_SIMPLE_PAIRING_RANDOMIZER_R = 0x0F,
  GAP_DATATYPE_SIMPLE_PAIRING_RANDOMIZER_R_192 = 0x0F,
  GAP_DATATYPE_DEVICE_ID = 0x10,
  GAP_DATATYPE_SECURITY_MANAGER_TK_VALUE = 0x10,
  GAP_DATATYPE_SECURITY_MANAGER_OUT_OF_BAND_FLAGS = 0x11,
  GAP_DATATYPE_SLAVE_CONNECTION_INTERVAL_RANGE = 0x12,
  GAP_DATATYPE_LIST_OF_16_BIT_SERVICE_SOLICITATION_UUIDS = 0x14,
  GAP_DATATYPE_LIST_OF_32_BIT_SERVICE_SOLICITATION_UUIDS = 0x1F,
  GAP_DATATYPE_LIST_OF_128_BIT_SERVICE_SOLICITATION_UUIDS = 0x15,
  GAP_DATATYPE_SERVICE_DATA = 0x16,
  GAP_DATATYPE_SERVICE_DATA_16_BIT_UUID = 0x16,
  GAP_DATATYPE_SERVICE_DATA_32_BIT_UUID = 0x20,
  GAP_DATATYPE_SERVICE_DATA_128_BIT_UUID = 0x21,
  GAP_DATATYPE_LE_SECURE_CONNECTIONS_CONFIRMATION_VALUE = 0x22,
  GAP_DATATYPE_LE_SECURE_CONNECTIONS_RANDOM_VALUE = 0x23,
  GAP_DATATYPE_URI = 0x24,
  GAP_DATATYPE_INDOOR_POSITIONING = 0x25,
  GAP_DATATYPE_TRANSPORT_DISCOVERY_DATA = 0x26,
  GAP_DATATYPE_PUBLIC_TARGET_ADDRESS = 0x17,
  GAP_DATATYPE_RANDOM_TARGET_ADDRESS = 0x18,
  GAP_DATATYPE_APPEARANCE = 0x19,
  GAP_DATATYPE_ADVERTISING_INTERVAL = 0x1A,
  GAP_DATATYPE_LE_BLUETOOTH_DEVICE_ADDRESS = 0x1B,
  GAP_DATATYPE_LE_ROLE = 0x1C,
  GAP_DATATYPE_SIMPLE_PAIRING_HASH_C_256 = 0x1D,
  GAP_DATATYPE_SIMPLE_PAIRING_RANDOMIZER_R_256 = 0x1E,
  GAP_DATATYPE_3D_INFORMATION_DATA = 0x3D,
  GAP_DATATYPE_MANUFACTURER_SPECIFIC_DATA = 0xFF,
  GAP_DATATYPE_LE_SUPPORTED_FEATURES = 0x27,
  GAP_DATATYPE_CHANNEL_MAP_UPDATE_INDICATION = 0x28
};

class GAPAdvertisingDataBuilder {

  private:

  public:

    GAPAdvertisingDataBuilder();
};

#endif /* _GAP_ADVERTISING_DATA_BUILDER_H_ */