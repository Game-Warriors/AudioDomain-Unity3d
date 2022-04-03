# AudioDomain-Unity3d
The audio system provide structure to manage one shot and background loop audio in scene.
These fuctionalities provided by two interfaces in audio domain package.
</br>1-IAudioEffect: this interface using for playing ono shot audio in different position in scene.
</br>2-IAudioLoop: this interface using for playing background looping audio in different layer.
</br>There is default audio system implementation in package which has basic logic for interfaces.
You can register IAudio Effect and IAudioLoop in your DI By single default audio system implemention or create and use you own audio system.
</br>This is the our DI package which is using in our projects: https://github.com/Game-Warriors/DependencyInjection-Unity3d

The default audio system need some resources for it's logics.
The sample resource could be found in sample folder in package. The Asset must be assign in audio system editor which is in "Tools -> Audio Configuration" in top bar menus in unity3d editor.
