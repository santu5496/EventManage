const CACHE_NAME = 'event-mgmt-v3';
const urlsToCache = [
  '/',
  '/css/site.css',
  '/Style/common.css',
  '/js/site.js',
  '/js/common.js',
  '/Image/LogoImg.png',
  '/Image/CompanyLogo.png'
];

self.addEventListener('install', event => {
  event.waitUntil(
    caches.open(CACHE_NAME)
      .then(cache => {
        console.log('Opened cache');
        return cache.addAll(urlsToCache);
      })
      .catch(err => {
        console.log('Cache failed:', err);
      })
  );
  self.skipWaiting();
});

self.addEventListener('fetch', event => {
  if (event.request.method !== 'GET') {
    return;
  }

  // Don't intercept external CDN requests
  if (event.request.url.includes('cdn.') || 
      event.request.url.includes('cdnjs.') ||
      event.request.url.includes('googleapis.') ||
      event.request.url.includes('cdn.jsdelivr.net') ||
      event.request.url.includes('api.') ||
      event.request.url.startsWith('https://')) {
    // Let external resources load without caching
    return;
  }

  event.respondWith(
    caches.match(event.request)
      .then(response => {
        if (response) {
          return response;
        }
        return fetch(event.request).then(response => {
          if (!response || response.status !== 200 || response.type !== 'basic') {
            return response;
          }
          if (event.request.url.includes('/api/') || 
              event.request.url.includes('/Home/') ||
              event.request.method !== 'GET') {
            return response;
          }
          const responseToCache = response.clone();
          caches.open(CACHE_NAME)
            .then(cache => {
              cache.put(event.request, responseToCache);
            })
            .catch(err => {
              console.log('Failed to cache:', err);
            });
          return response;
        }).catch(err => {
          console.log('Fetch failed:', err);
          return caches.match('/').catch(() => {
            return new Response('Offline');
          });
        });
      })
      .catch(err => {
        console.log('Cache match failed:', err);
        return fetch(event.request).catch(() => {
          return new Response('Offline');
        });
      })
  );
});

self.addEventListener('activate', event => {
  event.waitUntil(
    caches.keys().then(cacheNames => {
      return Promise.all(
        cacheNames.filter(cacheName => {
          return cacheName !== CACHE_NAME;
        }).map(cacheName => {
          return caches.delete(cacheName);
        })
      );
    })
  );
});
