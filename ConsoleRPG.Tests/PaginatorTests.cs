namespace ConsoleRPG.Tests
{
    public class PaginatorTests
    {
        [Theory]
        [InlineData(0, 1)]
        [InlineData(10, 3)]
        [InlineData(8, 2)]
        public void GetTotalPages_WithDifferentTotalItemsCount_BetweenOneAndTotalPages(int TotalItemsCount, int result)
        {
            var paginator = new Paginator<int>(4);
            var totalPages = paginator.GetTotalPages(TotalItemsCount);
            Assert.Equal(result, totalPages);
        }

        [Fact]
        public void NextPage_BeyondLastPage_StaysOnLatsPage()
        {
            var paginator = new Paginator<int>(4);
            Assert.Equal(1, paginator.CurrentPage);

            paginator.NextPage(10);
            paginator.NextPage(10);
            paginator.NextPage(10);
            paginator.NextPage(10);

            Assert.Equal(3, paginator.CurrentPage);
        }

        [Fact]
        public void PreviousPage_OnFirstPage_StaysOnFirstPage()
        {
            var paginator = new Paginator<int>(4);
            Assert.Equal(1, paginator.CurrentPage);

            paginator.PreviousPage();
            paginator.PreviousPage();

            Assert.Equal(1, paginator.CurrentPage);
        }

        [Fact]
        public void GetPage_OnFirstPage_FirstlyFourItems()
        {
            var paginator = new Paginator<int>(4);
            List<int> listOfItems =
            [
                1, 11, 12, 13, 34, 45, 67, 145, 2, 9
            ];

            List<int> elements = paginator.GetPage(listOfItems);

            Assert.Equal([1, 11, 12, 13], elements);
        }

        [Fact]
        public void GetPage_OnLastNotFullPage_LastItems()
        {
            var paginator = new Paginator<int>(4);
            List<int> listOfItems =
            [
                1, 11, 12, 13, 34, 45, 67, 145, 2, 9
            ];

            paginator.NextPage(10);
            paginator.NextPage(10);
            paginator.NextPage(10);
            paginator.NextPage(10);

            var elements = paginator.GetPage(listOfItems);
            Assert.Equal([2, 9], elements);
        }

        [Fact]
        public void GetPage_ChangeListCount_CurrentPageChangeToOne()
        {
            var paginator = new Paginator<int>(4);
            List<int> listOfItems =
            [
                1, 11, 12, 13, 34, 45, 67, 145, 2, 9
            ];

            paginator.NextPage(10);
            paginator.NextPage(10);
            paginator.NextPage(10);
            paginator.NextPage(10);

            listOfItems = [11, 12, 13, 14];

            var elements = paginator.GetPage(listOfItems);
            Assert.Equal([11, 12, 13, 14], elements);
            Assert.Equal(1, paginator.CurrentPage);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ConstructorParametrItemsPerPage_IsNegativeOrZero_ThrowException(int ItemsPerPage)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Paginator<int>(ItemsPerPage));
        }
    }
}