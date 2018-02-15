# Scraper
A simple scraper API

## About
This is a pretty simple scraper with no persistence. All jobs will be stored in an InMemoryRepository (ConcurrentDictionary) for retrieval at any time. The Scraper is also pretty basic in that it only uses HttpClient with limited condition handling and then AngleSharp for html parsing.  All jobs are processed via the JobManager.  The JobManager kicks off a long-running thread which watches a Queue for jobs.  Assuming there's processing room (IJobManager.MaxScrapers) and items in the Queue, they will be kicked off for processing.  

Simply running the project defaults to the auto generated API docs (Microsoft.AspNet.WebApi.HelpPage)

## How To Use
This is not a full list of API calls, just the basics to get going;

- Make a **POST** request to: http://localhost:65304/scraper/scrape with a body of: 
```json
    {
        url: "https://www.google.com",
        selectors: ["h1", "h2"]
    }
```    
    - This will return an object with the job's Id.
- Make a **GET** request to http://localhost:65304/scraper/job/[jobId]
    - Replace [jobId] with the Id returned above.
- Check the (paginated) results of jobs based on status by making a **GET** request to the following URLs
    - http://localhost:65304/scraper/pending?numRecords=10&pageNumber=1
    - http://localhost:65304/scraper/running
    - http://localhost:65304/scraper/failed
    - http://localhost:65304/scraper/completed
- Update the JobManagers state by making **POST** requests to the following endpoints
    - http://localhost:65304/admin/pause
    - http://localhost:65304/admin/resume
    - http://localhost:65304/admin/stop
    - http://localhost:65304/admin/start

## Scraper.LoadTester
This is a simple console app which makes some Scrape requests against the API.  There's a UrlList.txt file with URLs to scrape, and it just pulls out h1,h2,h3 tags.  It doesn't have a lot of URLs but it will simplify adding some jobs to view throughout the API.

## Improvements
1. Using an in-memory data store is not ideal since any time the app shuts down it's lost.  A database/document store would make this much better.  I've used the Repository pattern so this type of change could be (more) easily made in the future
2. JobManager currently kicks off a long-running thread which is at the whim of IIS (i.e. it can be killed whenever which is a major problem.)  Frameworks such as [Quartz.net](https://www.quartz-scheduler.net/) and [Hangfire](https://www.hangfire.io/) could really help with this as they have persistence. 
3. A solution to would be using message brokering (i.e. Kafka, NServiceBus, etc). This would ensure horizontal scaling to handle larger loads.
    1. The API would create a Scrape message.
    2. Handlers (ideally Lambda or another serverless architecture) could pick up the message, do the work and send a Response
    3. Response could be persisted/etc
    4. This simplifies creating a workflow to break up IScraperService
        1. Send message to get HTML
        2. After response, send message to Scrape, etc.
4. Furthering the scraper by adding Crawling functionality would be cool
    1. Add a new CrawlerController/CrawlerService which wraps up the ScraperService.
    2. Parses the results from ScraperService (i.e. [HtmlAgilityPack](http://html-agility-pack.net/) or the like) to grab all links on the domain.
5. With SPA trends, being able to crawl JavaScript rendered pages could be ideal. This simple crawler will not process any dynamically loaded content. 
