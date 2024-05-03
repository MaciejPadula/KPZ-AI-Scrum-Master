import {
  HttpHandlerFn,
  HttpHeaders,
  HttpInterceptorFn,
  HttpRequest,
} from '@angular/common/http';

export const cacheInterceptor: HttpInterceptorFn = (
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  req: HttpRequest<any>,
  next: HttpHandlerFn
) => {
  const oldHeaders = req.headers;
  let newHeaders = new HttpHeaders({
    'Cache-Control': 'no-cache',
    Pragma: 'no-cache',
    Expires: 'Sat, 01 Jan 2000 00:00:00 GMT',
  });

  oldHeaders.keys().forEach((key) => {
    const value = oldHeaders.get(key);
    if (value === null || newHeaders.has(key)) {
      return;
    }
    newHeaders = newHeaders.append(key, value);
  });


  const httpRequest = req.clone({
    headers: newHeaders
  });

  return next(httpRequest);
};
