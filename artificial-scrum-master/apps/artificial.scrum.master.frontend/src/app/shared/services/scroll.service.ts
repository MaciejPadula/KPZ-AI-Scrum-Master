import { ElementRef, Injectable } from '@angular/core';
import { take, timer } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ScrollService {
  public scrollToElement({
    element,
    timeout = 0,
    block = 'end',
  }: {
    element: ElementRef;
    timeout?: number;
    block?: 'center' | 'end' | 'nearest' | 'start';
  }) {
    if (element?.nativeElement) {
      const scrollOptions: ScrollIntoViewOptions = {
        behavior: 'smooth',
        block: block as ScrollLogicalPosition,
        inline: 'nearest',
      };
      const scrollAction = () =>
        element.nativeElement.scrollIntoView(scrollOptions);

      if (timeout > 0) {
        timer(timeout).pipe(take(1)).subscribe(scrollAction);
      } else {
        scrollAction();
      }
    }
  }
}
