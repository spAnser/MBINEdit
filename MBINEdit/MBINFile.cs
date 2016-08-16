using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBINEdit
{
    class MBINFile
    {
        public MBINHeader Header;
        private readonly IO _io;
        private readonly string _filePath;

        public MBINFile(string path)
        {
            _filePath = path;
            _io = new IO(path);
        }

        public bool Load()
        {
            _io.Stream.Position = 0;
            Header = _io.Reader.ReadStruct<MBINHeader>();
            return true;
        }

        public bool Save()
        {
            _io.Stream.Position = 0;
            _io.Writer.WriteStruct(Header);
            _io.Stream.Flush();

            return true;
        }

        public object GetData()
        {
            if (Header == null || String.IsNullOrEmpty(Header.TemplateName))
                return null;

            _io.Stream.Position = 0x60; // MBIN data start

            switch(Header.TemplateName)
            {
                case "cGcDebugOptions": // compiled GcDebugOptions
                    return _io.Reader.ReadStruct<cGcDebugOptions>();
                case "cGcUserSettingsData":
                    return _io.Reader.ReadStruct<cGcUserSettingsData>();
                case "cTkGraphicsSettings":
                    return _io.Reader.ReadStruct<cTkGraphicsSettings>();
                case "cTkEngineSettingsMapping":
                    return _io.Reader.ReadStruct<cTkEngineSettingsMapping>();
                case "cGcWaterGlobals":
                    return _io.Reader.ReadStruct<cGcWaterGlobals>();
                case "cGcAtlasGlobals":
                    return _io.Reader.ReadStruct<cGcAtlasGlobals>();
                case "cGcBootLogoData":
                    return _io.Reader.ReadStruct<cGcBootLogoData>();
                case "cGcHUDComponent": // don't think any MBIN files use this template as the base template, but templates do use it in lists (which aren't deserialized by MBINEdit... yet...)
                    return _io.Reader.ReadStruct<cGcHUDComponent>();
            }

            return null; // struct/template not mapped yet
        }

        public void SetData(object obj)
        {
            if (Header == null || String.IsNullOrEmpty(Header.TemplateName))
                return;

            // todo: don't trim MBIN if the template isn't known
            _io.Stream.SetLength(0x60); // trim data from the MBIN
            _io.Stream.Position = 0x60;

            switch(obj.GetType().Name)
            {
                case "cGcDebugOptions":
                    _io.Writer.WriteStruct((cGcDebugOptions)obj);
                    break;
                case "cGcUserSettingsData":
                    _io.Writer.WriteStruct((cGcUserSettingsData)obj);
                    break;
                case "cTkGraphicsSettings":
                    _io.Writer.WriteStruct((cTkGraphicsSettings)obj);
                    break;
                case "cTkEngineSettingsMapping":
                    _io.Writer.WriteStruct((cTkEngineSettingsMapping)obj);
                    break;
                case "cGcWaterGlobals":
                    _io.Writer.WriteStruct((cGcWaterGlobals)obj);
                    break;
                case "cGcAtlasGlobals":
                    _io.Writer.WriteStruct((cGcAtlasGlobals)obj);
                    break;
                case "cGcBootLogoData":
                    _io.Writer.WriteStruct((cGcBootLogoData)obj);
                    break;
                case "cGcHUDComponent":
                    _io.Writer.WriteStruct((cGcHUDComponent)obj);
                    break;
            }
        }
    }
}
