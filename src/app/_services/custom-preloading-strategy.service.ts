//import 'rxjs/add/observable/of';
import { Injectable } from '@angular/core';
import { PreloadingStrategy, Route } from '@angular/router';
import { Observable, of } from 'rxjs';

@Injectable()
export class CustomPreloadingStrategyService implements PreloadingStrategy {
  // preloadedModules: string[] = [];

  public preload(route: Route, load: () => Observable<any>): Observable<any> {
    // if(route.data && route.data['preload']) {
    //   this.preloadedModules.push(route.path);console.log('ss');
    //   return load();
    // } else {console.log('null');
    //   return of(null);
    // }

    return route.data && route.data['preload'] ? load() : of(null);
  }
}