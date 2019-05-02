using LibHac;
using LibHac.IO;
using LibHac.IO.RomFs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Nintendo.Switch
{
    public class NcaWrapper : IDisposable
    {
        private FileStream fileStream;
        private FileStream updateFileStream;
        private Nca baseNca;
        private Nca updateNca = null;

        private Nca nca
        {
            get
            {
                return updateNca == null ? baseNca : updateNca;
            }
        }

        public PartitionFileSystem Exefs
        {
            get;
            private set;
        }

        public RomFsFileSystem Romfs
        {
            get;
            private set;
        }

        public NcaWrapper(Keyset keyset, string ncaPath, string updateNcaPath = null)
        {
            // Open the FileStream
            fileStream = File.OpenRead(ncaPath);

            // Create the Nca instance
            baseNca = new Nca(keyset, fileStream.AsStorage(), true);

            // Open the update NCA if it exists
            if (updateNcaPath != null)
            {
                // Open the FileStream
                updateFileStream = File.OpenRead(updateNcaPath);

                // Create the update Nca instance
                updateNca = new Nca(keyset, updateFileStream.AsStorage(), true);

                // Set the baseNca
                updateNca.SetBaseNca(baseNca);
            }

            // Open the exefs
            Exefs = new PartitionFileSystem(nca.OpenSection(ProgramPartitionType.Code, false, IntegrityCheckLevel.ErrorOnInvalid, false));

            // Open the romfs
            Romfs = new RomFsFileSystem(nca.OpenSection(ProgramPartitionType.Data, false, IntegrityCheckLevel.ErrorOnInvalid, false));
        }

        public void Dispose()
        {
            fileStream.Dispose();

            if (updateFileStream != null)
            {
                updateFileStream.Dispose();
            }
        }

    }
}