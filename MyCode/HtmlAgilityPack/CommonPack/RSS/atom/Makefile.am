CSC = mcs -debug
ASSEMBLY = atom.dll

ATOM_CSFILES = ./AssemblyInfo.cs \
./Atom.AdditionalElements/ScopedElement.cs \
./Atom.AdditionalElements/ScopedElementCollection.cs \
./Atom.AdditionalElements/DublinCore/DcElement.cs \
./Atom.Core/AtomContentConstruct.cs \
./Atom.Core/AtomContent.cs \
./Atom.Core/AtomDateConstruct.cs \
./Atom.Core/AtomElement.cs \
./Atom.Core/AtomEntry.cs \
./Atom.Core/AtomFeed.cs \
./Atom.Core/AtomGenerator.cs \
./Atom.Core/AtomLink.cs \
./Atom.Core/AtomPersonConstruct.cs \
./Atom.Core/Enums.cs \
./Atom.Core/Exceptions.cs \
./Atom.Core.Collections/AtomContentCollection.cs \
./Atom.Core.Collections/AtomEntryCollection.cs \
./Atom.Core.Collections/AtomLinkCollection.cs \
./Atom.Core.Collections/AtomPersonConstructCollection.cs \
./Atom.Utils/DefaultValues.cs \
./Atom.Utils/Utils.cs \
./AtomReader.cs \
./AtomWriter.cs

build_sources = $(addprefix $(srcdir)/, $(ATOM_CSFILES))

atomlibdir = $(prefix)/lib/GtkSharpRSS
atomlib_DATA = $(ASSEMBLY)

all: $(ASSEMBLY)

DLLS = /r:System.Web.dll

$(ASSEMBLY): $(build_sources)
	$(CSC) /target:library $(build_sources) $(DLLS) /out:$(ASSEMBLY)

EXTRA_DIST = $(ATOM_CSFILES)

CLEANFILES = $(ASSEMBLY)

