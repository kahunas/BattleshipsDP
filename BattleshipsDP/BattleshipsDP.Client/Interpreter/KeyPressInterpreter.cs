
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace BattleshipsDP.Client.Interpreter;
public class KeyPressInterpreter : IAsyncDisposable
{
    private readonly IJSRuntime _jsRuntime;
    private DotNetObjectReference<KeyPressInterpreter> _dotNetRef;

    public event Func<Task> OnSpacebarPressed;

    public KeyPressInterpreter(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task InitializeAsync()
    {
        _dotNetRef = DotNetObjectReference.Create(this);
        await _jsRuntime.InvokeVoidAsync("addSpacebarListener", _dotNetRef);
    }

    [JSInvokable]
    public async Task TriggerSpacebar()
    {
        if (OnSpacebarPressed != null)
        {
            await OnSpacebarPressed.Invoke();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_dotNetRef != null)
        {
            await _jsRuntime.InvokeVoidAsync("removeSpacebarListener");
            _dotNetRef.Dispose();
        }
    }
}