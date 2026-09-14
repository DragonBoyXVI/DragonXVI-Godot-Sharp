using System.Text.Json;
using Godot;

namespace DragonXVI.XVIGodot.TranslationSystem;

/// <summary>
/// This class is designed to work around the (in my opinion) clunky Godot translation system.
/// Simply provide this class with a json file, or a directory of files to parse, and it will fill out everything just like godot would.
/// 
/// This class also allows mon string data to be stored, like arrays of messages or numbers.
/// This allows you to have different random messages or paramaters per language, how you use it is up to you!
/// </summary>
public static class TranslationImporter
{
    private const string LocaleKey = "locale";
    
    // avoid godot types when possible, we only need the translation object to talk to the engine.
    // inner dicts are godot ones, as they are stored right from godots json parser
    // Variant is nice here bc we can store anything in that block
    // locale: datadict<key,variant>
    private static readonly Dictionary<string, Dictionary<string, Variant>> ExtraData = [];

    /// <summary>
    /// Returns a value form the extra data cache.
    /// </summary>
    /// <param name="dataKey">String key for the data you want.</param>
    /// <param name="locale">Optonal locale. By default it uses the current one from the <see cref="TranslationServer"/>.</param>
    /// <returns></returns>
    public static Variant? GetExtraData( string dataKey, string? locale = null )
    {
        locale ??= TranslationServer.GetLocale();
        if ( !ExtraData.TryGetValue( locale, out var dataDict ) )
        {
            GD.PushError( $"No extra data for this locale: {locale}" );
            return null;
        }
        if ( dataDict.TryGetValue( dataKey, out var value ) )
        {
            GD.PushError( $"No extra data key for this locale: {locale}, key: {dataKey}" );
            return null;
        }
        return value;
    }
    
    /// <summary>
    /// This method takes a dictionary, usually one parsed from json, and turns it into a Godot.Translation object.
    /// This translation object can be added to the translation server after.
    /// This also generates an extra data dictionary for any values that arent strings, and adds them to its own extra data cache for you to read later.
    /// </summary>
    /// <param name="dict">The dict to parse into a translation.</param>
    /// <param name="extraDataDict">Return for the extra data dict. Feel free to discard this return.</param>
    /// <returns></returns>
    public static Translation? ParseDictToTranslation( Dictionary<string, Variant> dict, out Dictionary<string, Variant>? extraDataDict )
    {
        if ( !dict.TryGetValue(LocaleKey, out var value) )
        {
            GD.PushError( "Invalid dict! No locale key!" );
            extraDataDict = null;
            return null;
        }
        string locale = value.AsString();
        Translation translation = new(){
            Locale = locale
        };
        
        if ( !ExtraData.TryGetValue( locale, out extraDataDict ) )
        {
            extraDataDict = [];
        }
        
        foreach (var entry in dict)
        {
            if ( entry.Key == LocaleKey ) continue;
            if ( entry.Value.VariantType == Variant.Type.String )
            {
                translation.AddMessage( entry.Key, entry.Value.AsString() );
            }
            else
            {
                extraDataDict[ entry.Key ] = entry.Value;
            }
        }
        
        ExtraData[ locale ] = extraDataDict;
        return translation;
    }
    /// <summary>
    /// Takes a path to a file, then parses that file for a dictionary and returns it.
    /// This can return null, if it does the reason why will be in Godots terminal.
    /// </summary>
    /// <param name="filePath">Path to the file you want parsed.</param>
    /// <returns><see cref="Dictionary"/> (Csharp) of string keys and [<see cref="Variant"/>] values.</returns>
    public static Dictionary<string, Variant>? ParseFileForDict( string filePath )
    {
        if ( !Godot.FileAccess.FileExists( filePath ) )
        {
            GD.PushError( $"Invalid file path: {filePath}" );
            return null;
        }
        
        Godot.FileAccess? file = Godot.FileAccess.Open( filePath, Godot.FileAccess.ModeFlags.Read );
        if ( file is null )
        {
            GD.PushError( $"File open error! Path: {filePath}, Error: {Godot.FileAccess.GetOpenError()}" );
            return null;
        }
        
        string fileString = file.GetAsText();
        file.Close();
        file.Dispose();
        
        Dictionary<string, Variant>? parsedDict = JsonSerializer.Deserialize<Dictionary<string, Variant>>(fileString);
        if ( parsedDict is null )
        {
            GD.PushError( $"JSON parse failed! Path: {filePath}" );
            return null;
        }
        
        return parsedDict;
    }

    /// <summary>
    /// Parses a folder and all its subfolders (if searchSubdirs is true) for translation json files.
    /// Also adds them to the translation server.
    /// </summary>
    /// <param name="dirPath"></param>
    /// <param name="searchSubdirs"></param>
    public static void ParseDirForFiles( string dirPath, bool searchSubdirs = true )
    {
        if ( DirAccess.DirExistsAbsolute( dirPath ) )
        {
            GD.PushError( $"Trying to parse a dir that doesnt exist! {dirPath}" );
            return;
        }
        
        DirAccess? dir = DirAccess.Open( dirPath );
        if ( dir is null )
        {
            GD.PushError($"Failded to open folder: {dirPath}, Error: {DirAccess.GetOpenError()}");
            return;
        }
        
        _ = dir.ListDirBegin();
        string fileName = dir.GetNext();
        while ( fileName != "" )
        {
            if ( dir.CurrentIsDir() && searchSubdirs ) {
                ParseDirForFiles( dir.GetCurrentDir() + "/" + fileName );
            }
            else
            {
                var dict = ParseFileForDict( dir.GetCurrentDir() + "/" + fileName );
                if ( dict is null )
                {
                    continue;
                }
                var translation = ParseDictToTranslation( dict, out var _ );
                TranslationServer.AddTranslation( translation );
            }
            
            fileName = dir.GetNext();
        }
    }
}