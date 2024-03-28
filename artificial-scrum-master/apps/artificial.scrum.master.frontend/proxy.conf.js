const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORTS
  ? `https://localhost:${env.ASPNETCORE_HTTPS_PORTS}`
  : env.ASPNETCORE_URLS
  ? env.ASPNETCORE_URLS.split(';')[0]
  : 'https://localhost:7271';

const PROXY_CONFIG = [
  {
    context: ['/api', '/settings'],
    target: target,
    secure: false,
  },
];

module.exports = PROXY_CONFIG;
