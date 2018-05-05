CSC = mcs -debug
ASSEMBLY = rss.dll
KEY = jluke.pub

RSS_CSFILES = AssemblyInfo.cs \
	      RssFeed.cs	\
	      RssModule.cs	\
	      RssModuleItem.cs	\
	      RssReader.cs	\
	      RssWriter.cs	\
	      Collections/ExceptionCollection.cs \
	      Collections/RssCategoryCollection.cs \
	      Collections/RssChannelCollection.cs \
	      Collections/RssFeedCollection.cs \
	      Collections/RssItemCollection.cs	\
	      Collections/RssModuleCollection.cs	\
	      Collections/RssModuleItemCollection.cs	\
	      Collections/RssModuleItemCollectionCollection.cs	\
	      RssChannel/RssChannel.cs	\
	      RssChannel/RssCloud.cs	\
	      RssChannel/RssImage.cs	\
	      RssChannel/RssTextInput.cs	\
	      RssItem/RssEnclosure.cs	\
	      RssItem/RssGuid.cs	\
	      RssItem/RssItem.cs	\
	      RssItem/RssSource.cs	\
	      RssModules/RssPhotoAlbum.cs \
	      Shared/DBBool.cs	\
	      Shared/RssCategory.cs	\
	      Shared/RssDefault.cs	\
	      Shared/RssElement.cs	\
	      Shared/RssEnumerators.cs

build_sources = $(addprefix $(srcdir)/, $(RSS_CSFILES))

rsslibdir = $(prefix)/lib/GtkSharpRSS
rsslib_DATA = $(ASSEMBLY)

all: $(ASSEMBLY)

$(KEY): $(srcdir)/../../jluke.pub
	cp $(srcdir)/../../jluke.pub $(KEY)

$(ASSEMBLY): $(KEY) $(build_sources)
	$(CSC) /target:library $(build_sources) /out:$(ASSEMBLY)

EXTRA_DIST = $(RSS_CSFILES)
CLEANFILES = $(ASSEMBLY)

clean-local:
	rm -f $(KEY)
