# Unity Save Serializer
This is a simple solution for data encryption / decryption and serialization / deserialization for your Unity games. Save your serialized classes via binary, json or xml and encrypt via AES/RSA etc.

## Features
This library can save / load your serialized data from XML, JSON, Binary or BSON file.<br/>
You can easly serialize your classes with DataProviders and encrypt / decrypt with different types.

**Allowed Types:**<br/>
- XML Serialization / Deserialization;
- JSON Serialization / Deserialization;
- Binary Serialization / Deserialization;
- BSON Serialization / Deserialization;

**Allowed Encryptors:**
- AES encryption / decryption;
- SHA1, SHA256, SHA512 hashing;
- RSA encryption / decryption;
- Base64 encryption / decryption;
- xxHash hashing;
- MD5 hashing;

## Example
```csharp
// Simple model for serialization / deserialization
[System.Serializable]
public class PlayerModel
{
    public Vector3 Position = new Vector3();
    public Quaternion Rotation = new Quaternion();
    public float Heal = 100f;
}

// Simple Save / Load
public class MyPlayerController
{
    private PlayerModel _playerData = new PlayerModel();

    // Class Constructor
    public MyPlayerController()
    {
        LoadData();
    }
    
    // Load Data
    public void LoadData()
    {
        
    }
    
    // Save Data
    public void SaveData()
    {
    
    }
}
```


## Authors
This library is developed by <a href="https://tinyplay.games/">TinyPlay Games</a> under MIT License.<br/>
Support contact: <a href="mailto:hi@tinyplay.games">hi@tinyplay.games</a>
