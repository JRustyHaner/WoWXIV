using System;
using SharpDX.XInput;
using WindowsInput.Native; 
using WindowsInput;
using System.Runtime.InteropServices;

namespace WoWXIV
{
    class Program
    {
        static void Main(string[] args)
        {

            // write program information
            Console.WriteLine("WoWXIV - World of Warcraft Xbox Controller Support in FF14 style");
            Console.WriteLine("Version 1.0.0");
            Console.WriteLine("By: Aemnas of Booty Bay Buccaneers");
            Console.WriteLine("===================");
            Console.WriteLine("To exit, press the back button on the XBOX controller");

            // List key mappings
            // Right Stick Camera
            // Left Stick Movement
            // A key confirm
            // B key escape
            // X key bags
            // Y key character
            // up key quests
            // down key map
            // left key tab target
            // right key cycle friendly targets
            // right trigger starts  action bars 3 and 4
            // left trigger starts action bars 1 and 2
            // RT + ABXY activates action bar 4, actions 1,2,3,4
            // RT + UP DOWN LEFT RIGHT activates action bar 3 actions 1,2,3,4
            // LT + ABXY activates action bar 2 actions 1,2,3,4
            // LT + UP DOWN LEFT RIGHT activates action bar 1 actions 1,2,3,4
            // Back Button closes this app
            // start button enables mouse on screen using right stick
            // Right shoulder to click on screen in mouse mode
            // Right shoulder marks skull in regular mode


            // import User32.dll
            [DllImport("user32.dll", SetLastError = true)]
            static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

            [DllImport("user32.dll")]
            static extern bool SetForegroundWindow(IntPtr hWnd);

            // check to see if the controller is connected
            var controller = new Controller(UserIndex.One);
            if (!controller.IsConnected)
            {
                Console.WriteLine("Controller not connected");
                return;
            }

            // check to see if world of warcraft is running
            var wow = FindWindow(null, "World of Warcraft");
            if (wow == IntPtr.Zero)
            {
                Console.WriteLine("World of Warcraft not running");
                return;
            } else
            {
                SetForegroundWindow(wow);
            }

            // create a new input simulator
            var inputSimulator = new InputSimulator();
            
            // create state of last fkey pressed
            var lastFKey = 0;

            // create menu state variables
            bool characterMenu = false;
            bool bagMenu = false;
            bool questMenu = false;
            bool mapMenu = false;
            bool mouseMode = false;
            bool rightTriggerActive = false;
            bool leftTriggerActive = false;


            // wait for the back button to be pressed
            while (true)
            {
                 // stick state variables
                bool rightStickActive = false;
                bool leftStickActive = false;
          

                var state = controller.GetState();
                if (state.Gamepad.Buttons == GamepadButtonFlags.Back)
                {
                    break;
                } 

                // if the a button is pressed
                if (state.Gamepad.Buttons == GamepadButtonFlags.A)
                {
                    // if the right trigger is active press 1, else press space
                    if (rightTriggerActive)
                    {
                        inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_1);
                    } else if(leftTriggerActive)
                    {
                        inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.SHIFT, VirtualKeyCode.VK_7);
                    } else
                    {
                        inputSimulator.Keyboard.KeyPress(VirtualKeyCode.SPACE);
                    }
                    // if the left trigger is active, press shift and 7

                }

                // if the b button is pressed
                if (state.Gamepad.Buttons == GamepadButtonFlags.B)
                {
                    // if the right trigger is active press 2, left tirgger press 6 else press escape
                    if (rightTriggerActive)
                    {
                        inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_2);
                    } else if (leftTriggerActive)
                    {
                        inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_6);
                    } else
                    {
                        inputSimulator.Keyboard.KeyPress(VirtualKeyCode.ESCAPE);
                        
                        //if the character menu is open close it using c
                        if (characterMenu)
                        {
                            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_C);
                            characterMenu = false;
                        }

                        //if the bag menu is open close it using b
                        if (bagMenu)
                        {
                            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_B);
                            bagMenu = false;
                        }

                        //if the quest menu is open close it using l
                        if (questMenu)
                        {
                            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_L);
                            questMenu = false;
                        }

                        //if the map menu is open close it using m
                        if (mapMenu)
                        {
                            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_M);
                            mapMenu = false;
                        }

                    }
                    //sleep to prevent double presses
                    System.Threading.Thread.Sleep(100);
                  
                }

                // if the x button is pressed
                if (state.Gamepad.Buttons == GamepadButtonFlags.X)
                {
                    // if the right trigger is active press 3, left tirgger press 7 else press b key
                    if (rightTriggerActive)
                    {
                        inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_3);
                    } else if (leftTriggerActive)
                    {
                        inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_7);
                    } else
                    {
                       //if the bag menu is closed, open it with b, otherwise do nothing
                        if (!bagMenu)
                        {
                            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_B);
                            bagMenu = true;
                            // prevent double presses
                            System.Threading.Thread.Sleep(100);
                        }
                    }
                }

                // if the y button is pressed
                if (state.Gamepad.Buttons == GamepadButtonFlags.Y)
                {
                    // if the right trigger is active press 4, left tirgger press 8 else press c key
                    if (rightTriggerActive)
                    {
                        inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_4);
                    } else if (leftTriggerActive)
                    {
                        inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_8);
                    } else
                    {
                        //if character menu is not open open it using c, otherwise do nothing
                        if (!characterMenu)
                        {
                            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_C);
                            characterMenu = true;
                            // prevent double presses
                            System.Threading.Thread.Sleep(100);
                        }
                    }
                }

                // if the down button is pressed
                if (state.Gamepad.Buttons == GamepadButtonFlags.DPadDown)
                {
                    // if the right trigger is active press 9, left tirgger press shift and 3 else press m key
                    if (rightTriggerActive)
                    {
                        inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_9);
                    } else if (leftTriggerActive)
                    {
                        inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.SHIFT, VirtualKeyCode.VK_1);
                    } else
                    {
                        //if map menu is not open open it using m, otherwise do nothing
                        if (!mapMenu)
                        {
                            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_M);
                            mapMenu = true;
                            // prevent double presses
                            System.Threading.Thread.Sleep(100);
                        }
                    } 
          
                }             
                // if the up button is pressed
                if (state.Gamepad.Buttons == GamepadButtonFlags.DPadUp)
                {
                    // if the right trigger is active press 0, left tirgger press shift and 4 else press l key
                    if (rightTriggerActive)
                    {
                        inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_0);
                    } else if (leftTriggerActive)
                    {
                        inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.SHIFT, VirtualKeyCode.VK_2);
                    } else
                    {
                        //if the quest menu is not open open it using l, otherwise do nothing
                        if (!questMenu)
                        {
                            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.VK_L);
                            questMenu = true;
                            // prevent double presses
                            System.Threading.Thread.Sleep(100);
                        }
                    }
                }  

                // if the left button is pressed
                if (state.Gamepad.Buttons == GamepadButtonFlags.DPadLeft)
                {
                    // if the right trigger is active press shift and 1, left tirgger press shift and 5 else press tab key
                    if (rightTriggerActive)
                    {
                        inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.SHIFT, VirtualKeyCode.VK_1);
                    } else if (leftTriggerActive)
                    {
                        inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.SHIFT, VirtualKeyCode.VK_5);
                    } else
                    {
                        inputSimulator.Keyboard.KeyPress(VirtualKeyCode.TAB);
                        lastFKey = -1;
                    }
                   
                }

                // if the right button is pressed
                VirtualKeyCode[] fKeys = new VirtualKeyCode[] { VirtualKeyCode.F1, VirtualKeyCode.F2, VirtualKeyCode.F3, VirtualKeyCode.F4, VirtualKeyCode.F5};
                
                if (state.Gamepad.Buttons == GamepadButtonFlags.DPadRight)
                {
                    // if the right trigger is active press shift and 2, left tirgger press shift and 6 else press tab key
                    if (rightTriggerActive)
                    {
                        inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.SHIFT, VirtualKeyCode.VK_2);
                    } else if (leftTriggerActive)
                    {
                        inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.SHIFT, VirtualKeyCode.VK_6);
                    } else
                    {
                        //press the next f key
                        lastFKey++;
                        if (lastFKey > 4)
                        {
                            lastFKey = 0;
                        }
                        inputSimulator.Keyboard.KeyPress(fKeys[lastFKey]);
                    }
                }

                // if the start button is pressed, toggle mouse mode and relese all mouse buttons
                if (state.Gamepad.Buttons == GamepadButtonFlags.Start)
                {
                    mouseMode = !mouseMode;
                    inputSimulator.Mouse.LeftButtonUp();
                    inputSimulator.Mouse.RightButtonUp();
                    //prevent repeated presses
                    System.Threading.Thread.Sleep(100);
                }
                

                // if the right bumper is pressed in mouse mode, press the right mouse button until released
                if (state.Gamepad.Buttons == GamepadButtonFlags.RightShoulder && mouseMode)
                {
                    inputSimulator.Mouse.LeftButtonDown();
                } else
                {
                    inputSimulator.Mouse.LeftButtonUp();
                }


                // if the right bumper is pressed out of mouse mode, press ctrl 1 until released
                if (state.Gamepad.Buttons == GamepadButtonFlags.RightShoulder && !mouseMode)
                {
                    inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_1);
                }
                
                // if the left trigger value is greater than 100, set the left trigger active to true
                if (state.Gamepad.LeftTrigger > 100)
                {
                    leftTriggerActive = true;
                } else
                {
                    leftTriggerActive = false;
                }
                
                // if the right trigger value is greater than 100, set the right trigger active to true
                if (state.Gamepad.RightTrigger > 100)
                {
                    rightTriggerActive = true;
                    // prevent repeated presses
                } else
                {
                    rightTriggerActive = false;
                }           

 

                // set the deadzone
                var deadzone = 25000;
                
                //check to see if the left stick is moved past the deadzone
                if(!rightStickActive){
                    if (state.Gamepad.LeftThumbX > deadzone || state.Gamepad.LeftThumbX < -deadzone || state.Gamepad.LeftThumbY > deadzone || state.Gamepad.LeftThumbY < -deadzone)
                    {
                    
                        // if the left stick is moved up and left past the deadzone, press the w and a keys until it is released
                        if (state.Gamepad.LeftThumbY < -deadzone && state.Gamepad.LeftThumbX < -deadzone)
                        {
                            inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.VK_A, VirtualKeyCode.VK_W);
                        }
                        
                        // if the left stick is moved up and right past the deadzone, press the d and w keys until it is released
                        if (state.Gamepad.LeftThumbY < -deadzone && state.Gamepad.LeftThumbX > deadzone)
                        {
                            inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.VK_D, VirtualKeyCode.VK_W);
                        }

                        // if the left stick is moved down and left past the deadzone, press the a and s keys until it is released
                        if (state.Gamepad.LeftThumbY > deadzone && state.Gamepad.LeftThumbX < -deadzone)
                        {
                            inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.VK_A, VirtualKeyCode.VK_S);
                        }

                        // if the left stick is moved down and right past the deadzone, press the d and s keys until it is released
                        if (state.Gamepad.LeftThumbY > deadzone && state.Gamepad.LeftThumbX > deadzone)
                        {
                            inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.VK_D, VirtualKeyCode.VK_S);
                        }

                        // if the left stick is moved left past the deadzone, press the a key until it is released
                        if (state.Gamepad.LeftThumbX < -deadzone)
                        {
                        inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_A);
                        }

                        // if the left stick is moved right past the deadzone, press the d key until it is released
                        if (state.Gamepad.LeftThumbX > deadzone)
                        {
                            inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_D);
                        }

                        // if the left stick is moved up past the deadzone, press the w key until it is released
                        if (state.Gamepad.LeftThumbY < -deadzone)
                        {
                            inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_S);
                        }

                        // if the left stick is moved down past the deadzone, press the s key until it is released
                        if (state.Gamepad.LeftThumbY > deadzone)
                        {
                            inputSimulator.Keyboard.KeyDown(VirtualKeyCode.VK_W);
                        }
                        
    

                    } else {
                        // if the left stick is not moved, release the a, d, w, and s keys
                        inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_A);
                        inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_D);
                        inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_W);
                        inputSimulator.Keyboard.KeyUp(VirtualKeyCode.VK_S);
                    }
                }

                deadzone = 1000;
                //check to see if the right stick is moved past the deadzone
                if(!leftStickActive){
                    if (state.Gamepad.RightThumbX > deadzone || state.Gamepad.RightThumbX < -deadzone || state.Gamepad.RightThumbY > deadzone || state.Gamepad.RightThumbY < -deadzone)
                    {
                        rightStickActive = true;
        
                        if (!mouseMode)
                        {
                            inputSimulator.Mouse.RightButtonDown();
                             var speed = 8000;

                            //move the mouse based on the right stick position
                            inputSimulator.Mouse.MoveMouseBy(state.Gamepad.RightThumbX / speed, state.Gamepad.RightThumbY / speed);
                        } else {
                            inputSimulator.Mouse.LeftButtonUp();
                            inputSimulator.Mouse.RightButtonUp();
                             var speed = 3000;

                            //move the mouse based on the right stick position
                             inputSimulator.Mouse.MoveMouseBy(state.Gamepad.RightThumbX / speed, state.Gamepad.RightThumbY / speed);
                        }
                        
                       
                    } else {
                        rightStickActive = false;
                        inputSimulator.Mouse.RightButtonUp();
                        inputSimulator.Mouse.LeftButtonUp();
                    }
                } else {
                     if (state.Gamepad.RightThumbX > deadzone || state.Gamepad.RightThumbX < -deadzone || state.Gamepad.RightThumbY > deadzone || state.Gamepad.RightThumbY < -deadzone)
                    {
                        rightStickActive = true;
        
                        if (!mouseMode)
                        {
                            inputSimulator.Mouse.RightButtonDown();
                            inputSimulator.Mouse.LeftButtonDown();
                        } else {
                            inputSimulator.Mouse.LeftButtonUp();
                            inputSimulator.Mouse.RightButtonUp();
                        }
                        
                        var speed = 8000;

                        //move the mouse based on the right stick position
                        inputSimulator.Mouse.MoveMouseBy(state.Gamepad.RightThumbX / speed, state.Gamepad.RightThumbY / speed);
                    } else {
                        rightStickActive = false;
                        inputSimulator.Mouse.RightButtonUp();
                        inputSimulator.Mouse.LeftButtonUp();
                    }
                }
                
                
               
                // wait for 10 milliseconds
                System.Threading.Thread.Sleep(15);

            }

        }
    }
}