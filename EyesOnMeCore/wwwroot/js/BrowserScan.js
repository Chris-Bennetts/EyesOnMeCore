function urlExists(url, cb) {
    jQuery.ajax({
        url: url,
        dataType: 'text',
        type: 'GET',
        complete: function (xhr) {
            if (typeof cb === 'function')
                cb.apply(this, [xhr.status]);
        }
    });
}

function browserscan() { 
urlExists('EXTENTIONURLHERE'.function(status) {
    if(status === 200) {
    console.log("extension exists");
} else if status === 400 {
    console.log("extension does not exist");
} else {
    console.log("extension check error")
    });
}
