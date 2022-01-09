using Catharsium.Util.IO.Interfaces;
using Catharsium.Util.Testing;
using Catharsium.WhatsApp.Data._Configuration;
using Catharsium.WhatsApp.Data.Repositories;
using Catharsium.WhatsApp.Entities.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Threading.Tasks;
namespace Catharsium.WhatsApp.Data.Tests.Repositories;

[TestClass]
public class ConversationRepositoryTests : TestFixture<ConversationRepository>
{
    #region Fixture

    private static readonly string ConversationsFolder = "My conversations folder";

    private IFile[] Files { get; set; }

    private IDirectory Directory { get; set; }


    [TestInitialize]
    public void Initialize()
    {
        var settings = new WhatsAppDataSettings {
            ConversationsFolder = ConversationsFolder
        };
        this.SetDependency(settings);

        this.Files = new[] {
            Substitute.For<IFile>(), Substitute.For<IFile>()
        };
        this.Files[0].ExtensionlessName.Returns("My file 0");
        this.Files[0].Exists.Returns(true);
        this.Files[1].ExtensionlessName.Returns("My file 1");
        this.Files[1].Exists.Returns(true);

        this.Directory = Substitute.For<IDirectory>();
        this.Directory.GetFiles("*.json").Returns(this.Files);
        this.GetDependency<IFileFactory>().CreateDirectory(ConversationsFolder).Returns(this.Directory);
    }

    #endregion

    #region GetList

    [TestMethod]
    public async Task GetList_FolderDoesNotExist_CreatesFolder()
    {
        this.Directory.Exists.Returns(false);
        await this.Target.GetList();
        this.Directory.Received().Create();
    }


    [TestMethod]
    public async Task GetList_FolderExists_IsNotCreated()
    {
        this.Directory.Exists.Returns(true);
        await this.Target.GetList();
        this.Directory.DidNotReceive().Create();
    }


    [TestMethod]
    public async Task GetList_ReturnsAllFileNames()
    {
        this.Directory.Exists.Returns(true);
        var actual = await this.Target.GetList();
        Assert.AreEqual(this.Files.Length, actual.Count);
        foreach (var file in this.Files) {
            Assert.IsTrue(actual.Contains(file.ExtensionlessName));
        }
    }

    #endregion

    #region Get

    [TestMethod]
    public async Task Get_ValidName_ReturnsConversation()
    {
        var expected = new Conversation();
        this.GetDependency<IFileFactory>().CreateFile($"{ConversationsFolder}\\{this.Files[0].ExtensionlessName}.json").Returns(this.Files[0]);
        this.GetDependency<IJsonFileReader>().ReadFrom<Conversation>(this.Files[0]).Returns(expected);

        var actual = await this.Target.Get(this.Files[0].ExtensionlessName);
        Assert.IsNotNull(actual);
    }


    [TestMethod]
    public async Task Get_InvalidName_ReturnsNull()
    {
        this.GetDependency<IFileFactory>().CreateFile($"{ConversationsFolder}\\{this.Files[0].ExtensionlessName}.json").Returns(this.Files[0]);
        var actual = await this.Target.Get(this.Files[0].ExtensionlessName + "Other");
        Assert.IsNull(actual);
    }


    [TestMethod]
    public async Task Get_NonExistingFile_ReturnsNull()
    {
        var name = "My name";
        var file = Substitute.For<IFile>();
        file.Exists.Returns(false);
        var unexpected = new Conversation();
        this.GetDependency<IFileFactory>().CreateFile($"{ConversationsFolder}\\{name}.json").Returns(file);
        this.GetDependency<IJsonFileReader>().ReadFrom<Conversation>(file).Returns(unexpected);

        var actual = await this.Target.Get(name);
        Assert.IsNull(actual);
    }

    #endregion

    #region Save

    [TestMethod]
    public async Task Save_ExistingConversation_IsOverwritten()
    {
        var conversation = new Conversation {
            Name = "My name"
        };
        var file = Substitute.For<IFile>();
        file.Exists.Returns(true);
        this.GetDependency<IFileFactory>().CreateFile($"{ConversationsFolder}\\{conversation.Name}.json").Returns(file);

        await this.Target.Save(conversation);
        file.Received().Delete();
        this.GetDependency<IJsonFileWriter>().Write(conversation, file);
    }


    [TestMethod]
    public async Task Get_NewConversation_IsSaved()
    {
        var conversation = new Conversation {
            Name = "My name"
        };
        var file = Substitute.For<IFile>();
        file.Exists.Returns(false);
        this.GetDependency<IFileFactory>().CreateFile($"{ConversationsFolder}\\{conversation.Name}.json").Returns(file);

        await this.Target.Save(conversation);
        file.DidNotReceive().Delete();
        this.GetDependency<IJsonFileWriter>().Write(conversation, file);
    }

    #endregion
}