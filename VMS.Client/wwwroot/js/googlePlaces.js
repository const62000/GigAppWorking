function initializeAutocomplete(dotNetHelper) {
    const input = document.getElementById('autocomplete');

    if (input && input instanceof HTMLInputElement) {
        const autocomplete = new google.maps.places.Autocomplete(input);

        autocomplete.addListener('place_changed', () => {
            const place = autocomplete.getPlace();

            if (place && place.address_components && place.geometry) {
                const address = parseAddressComponents(place.address_components);
                address.latitude = place.geometry.location.lat(); // Add latitude
                address.longitude = place.geometry.location.lng(); // Add longitude
                dotNetHelper.invokeMethodAsync('OnPlaceSelected', address);
            }
        });
    } else {
        console.error('Autocomplete input element not found or not an HTMLInputElement');
    }
}

function parseAddressComponents(addressComponents) {
    const address = {
        street: '',
        city: '',
        country: '',
        latitude: null, // Add latitude
        longitude: null, // Add longitude
        postalCode: ''
    };


    for (const component of addressComponents) {
        if (component.types.includes('street_number')) {
            address.street = component.long_name;
        }
        if (component.types.includes('route')) {
            address.street += ` ${component.long_name}`;
        }
        if (component.types.includes('locality')) {
            address.city = component.long_name;
        }
        if (component.types.includes('country')) {
            address.country = component.long_name;
        }
        if (component.types.includes('postal_code')) {
            address.postalCode = component.long_name;
        }
    }


    return address;
}